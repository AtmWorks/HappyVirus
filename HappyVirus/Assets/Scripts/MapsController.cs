using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controlador de viaje entre mapas. Mantiene el estado de la run (mapa actual, bioma, openGates)
/// y se encarga de activar un mapa existente o instanciar uno nuevo según las reglas indicadas.
/// </summary>
public class MapsController : MonoBehaviour
{
    [Header("Catálogo de mapas (prefabs con MapSelector)")]
    public List<GameObject> maps = new List<GameObject>();

    [Header("Mapa del lobby (sin MapSelector)")]
    public GameObject lobbyMap;

    [Header("Estado de la run")]
    public GameObject currentMap;
    public GameObject player;
    public int difficulty = 1;
    public BiomeType currentBiome = BiomeType.Biome1;
    public bool isTransitionAvailable = false;
    public int openGates = 1;

    [Header("Límite de ramificación")]
    public int maxGatesPossible = 6;

    public List<GameObject> usedMaps = new List<GameObject>();
    public List<GameObject> originalMaps = new List<GameObject>();

    public GateBehavior lobbyGate;
    private void Start()
    {
        // Guardar copia de los mapas originales
        originalMaps = new List<GameObject>(maps);
    }

    /// <summary>
    /// Viaja al siguiente mapa. Puede reutilizar uno ya existente (linkedMap) o instanciar uno nuevo desde 'maps'.
    /// - fromOrientation: orientación por la que salimos del mapa actual (para colocar al jugador en la puerta opuesta).
    /// - gateFrom: la Gate desde la que hemos salido (en el mapa actual).
    /// - linkedMap: si no es null, ya existe y se reutiliza (no se instancia nuevo).
    /// - biomeToGo: si se especifica, el mapa nuevo debe pertenecer a ese bioma; si es null, debe coincidir con currentBiome.
    /// </summary>
    public void travelToNextMap(GateOrientation fromOrientation, GameObject gateFrom, GameObject linkedMap = null, BiomeType? biomeToGo = null)
    {
        var opposite = GetOpposite(fromOrientation);

        // --- CASO 1: ya existe un mapa enlazado (no se instancia nada) ---
        if (linkedMap != null)
        {
            // Activar el mapa destino y preparar teletransporte
            if (currentMap != null) currentMap.SetActive(false);
            linkedMap.SetActive(true);

            // Apagar player mientras recolocamos
            if (player != null) player.SetActive(false);

            // Encontrar la gate opuesta en el mapa destino usando su MapSelector.gates
            var ms = linkedMap.GetComponent<MapSelector>();
            if (ms == null)
            {
                Debug.LogWarning("[MapsController] linkedMap no tiene MapSelector.");
            }
            else
            {
                // Buscar una gate cuya orientación sea la opuesta
                var targetGateGO = ms.gates
                    .Select(go => go != null ? go.GetComponent<GateBehavior>() : null)
                    .Where(g => g != null && g.gateOrientation == opposite && g.spawnPoint != null)
                    .Select(g => g.spawnPoint)
                    .FirstOrDefault();

                if (targetGateGO != null && player != null)
                {
                    player.transform.position = targetGateGO.position;
                }
                else
                {
                    Debug.LogWarning("[MapsController] No se encontró Gate opuesta válida en linkedMap.");
                }

                // Reactivar player
                if (player != null) player.SetActive(true);

                // Actualizar estado
                currentMap = linkedMap;
                currentBiome = ms.biome;
            }

            return; // Termina aquí
        }

        // --- CASO 2: no hay mapa enlazado; instanciar uno nuevo siguiendo reglas ---

        // Resta 1 por usar una puerta de salida
        openGates --;

        // Necesitamos que el nombre del mapa contenga la puerta opuesta (L/R/T/B)
        char neededGateLetter = OrientationToLetter(opposite);

        // Comprobar bioma objetivo (si no se especifica, usamos currentBiome)
        BiomeType targetBiome = biomeToGo.HasValue ? biomeToGo.Value : currentBiome;

        // 1) Filtrar por bioma y por presencia de la gate necesaria en el NOMBRE
        var biomeCandidates = maps
            .Where(pf =>
            {
                if (pf == null) return false;
                var sel = pf.GetComponent<MapSelector>();
                if (sel == null) return false;
                if (sel.biome != targetBiome) return false;

                string n = pf.name.ToUpperInvariant();
                return n.Contains(neededGateLetter.ToString());
            })
            .ToList();

        if (biomeCandidates.Count == 0)
        {
            Debug.LogWarning("[MapsController] No hay mapas candidatos válidos para el bioma/orientación requeridos.");
            return;
        }

        // 2) Determinar si debemos forzar DeadEnd según openGates y maxGatesPossible
        bool mustForceDeadEnd = openGates > maxGatesPossible;

        // 3) Separar candidatos en DeadEnds vs No-DeadEnds (por nombre)
        var deadEnds = biomeCandidates.Where(IsDeadEndName).ToList();
        var notDeadEnds = biomeCandidates.Where(p => !IsDeadEndName(p.name)).ToList();

        // 4) Seleccionar un prefab válido según la regla de dead end
        GameObject chosenPrefab = null;

        if (mustForceDeadEnd)
        {
            if (deadEnds.Count == 0)
            {
                // Si no hay deadEnds disponibles, caemos a cualquier candidato (para no bloquear)
                chosenPrefab = biomeCandidates[Random.Range(0, biomeCandidates.Count)];
            }
            else
            {
                chosenPrefab = deadEnds[Random.Range(0, deadEnds.Count)];
            }
        }
        else
        {
            // Podemos usar cualquier no-deadEnd; si no hay, usar deadEnd
            if (notDeadEnds.Count > 0)
                chosenPrefab = notDeadEnds[Random.Range(0, notDeadEnds.Count)];
            else
                chosenPrefab = deadEnds[Random.Range(0, deadEnds.Count)];
        }

        if (chosenPrefab == null)
        {
            Debug.LogWarning("[MapsController] No se pudo elegir un prefab de mapa.");
            return;
        }

        // 5) Actualizar openGates sumando (numGatesDelMapa - 1) si no es deadEnd
        int gateCount = CountGateLettersFromName(chosenPrefab.name);
        if (!IsDeadEndName(chosenPrefab.name))
        {
            // Contamos todas menos la de entrada
            openGates += Mathf.Max(0, gateCount - 1);
        }
        // Si es deadEnd, no se suma nada (ya restamos 1 al salir)

        // 6) Instanciar el mapa y activar variante
        var instance = Instantiate(chosenPrefab);
        instance.name = chosenPrefab.name + "_Instance";

        // Apagar currentMap y player antes de posicionar
        if (currentMap != null) currentMap.SetActive(false);
        if (player != null) player.SetActive(false);

        var selector = instance.GetComponent<MapSelector>();
        if (selector == null)
        {
            Debug.LogWarning("[MapsController] El mapa instanciado no tiene MapSelector.");
            if (player != null) player.SetActive(true);
            return;
        }

        // Ejecutar selección de variante (por ahora VariantType.Default)
        var gatesList = selector.variantSelector(VariantType.Default, difficulty, isTransitionAvailable);
        if (gatesList == null || gatesList.Count == 0)
        {
            Debug.LogWarning("[MapsController] La variante activada no devolvió gates.");
            if (player != null) player.SetActive(true);
            return;
        }

        // 7) Elegir una Gate válida:
        //    - GateBehavior.linkedGate y linkedMap vacías
        //    - gateOrientation == opuesta a fromOrientation
        GateBehavior targetGate = null;

        foreach (var gateGO in gatesList)
        {
            if (gateGO == null) continue;
            var gb = gateGO.GetComponent<GateBehavior>();
            if (gb == null) continue;

            bool linksEmpty = (gb.linkedGate == null && gb.linkedMap == null);
            bool orientationOk = (gb.gateOrientation == opposite);

            if (linksEmpty && orientationOk)
            {
                targetGate = gb;
                break;
            }
        }

        if (targetGate == null)
        {
            Debug.LogWarning("[MapsController] No se encontró una Gate válida en el mapa instanciado.");
            if (player != null) player.SetActive(true);
            return;
        }

        // 8) Linkear las puerta de origen y destino entre sí y sus mapas 
        targetGate.linkedGate = gateFrom;
        targetGate.linkedMap = currentMap;

        gateFrom.GetComponent<GateBehavior>().linkedGate = targetGate.gameObject;
        gateFrom.GetComponent<GateBehavior>().linkedMap = instance;


        // 9) Colocar al jugador en el SpawnPoint de la puerta elegida
        if (player != null && targetGate.spawnPoint != null)
        {
            player.transform.position = targetGate.spawnPoint.position;
        }

        // 10) Encender player y actualizar estado
        if (player != null) player.SetActive(true);

        instance.SetActive(true);
        currentMap = instance;
        currentBiome = selector.biome;

        // 11) Eliminar el prefab elegido de la lista de mapas disponibles (ya forma parte de la run)
        maps.Remove(chosenPrefab);
        usedMaps.Add(instance);
    }

    // ----------------- Helpers -----------------

    private static GateOrientation GetOpposite(GateOrientation o)
    {
        switch (o)
        {
            case GateOrientation.Left:   return GateOrientation.Right;
            case GateOrientation.Right:  return GateOrientation.Left;
            case GateOrientation.Top:    return GateOrientation.Bottom;
            case GateOrientation.Bottom: return GateOrientation.Top;
            default: return GateOrientation.Left;
        }
    }

    private static char OrientationToLetter(GateOrientation o)
    {
        switch (o)
        {
            case GateOrientation.Left:   return 'L';
            case GateOrientation.Right:  return 'R';
            case GateOrientation.Top:    return 'T';
            case GateOrientation.Bottom: return 'B';
            default: return 'L';
        }
    }

    private static bool IsDeadEndName(GameObject go) => IsDeadEndName(go.name);
    private static bool IsDeadEndName(string nameUpper)
    {
        return nameUpper.ToUpperInvariant().Contains("END");
    }

    /// <summary>
    /// Cuenta cuántas letras de {L,R,T,B} aparecen en el nombre (ej. "LRTB_1" -> 4, "LT_3" -> 2, "L_END_1" -> 1).
    /// </summary>
    private static int CountGateLettersFromName(string prefabName)
    {
        string n = prefabName.ToUpperInvariant();
        int count = 0;
        if (n.Contains("L")) count++;
        if (n.Contains("R")) count++;
        if (n.Contains("T")) count++;
        if (n.Contains("B")) count++;

        // Para nombres tipo "L_END_1" ya contará 1 por la L (correcto para deadEnd)
        return Mathf.Clamp(count, 0, 4);
    }

    public void DieReset()
    {
        currentMap = lobbyMap;//destroy the current map
        currentMap.SetActive(true);
        maps = new List<GameObject>(originalMaps);
        //destroy every map in usedMaps
        lobbyGate.linkedGate = null;
        lobbyGate.linkedMap = null;
        foreach (GameObject map in usedMaps)
        {
            Destroy(map);
        }
        usedMaps.Clear();
    }
}
