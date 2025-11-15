using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controlador de viaje entre mapas. Mantiene el estado de la run (mapa actual, bioma, openGates)
/// y se encarga de activar un mapa existente o instanciar uno nuevo según las reglas indicadas.
/// </summary>
public class MapsController : MonoBehaviour
{
    [Header("Rooms (>=3 puertas)")]
    public List<GameObject> roomMaps = new List<GameObject>();

    [Header("Tunnels (2 puertas)")]
    public List<GameObject> tunnelMaps = new List<GameObject>();

    [Header("Dead Ends (1 puerta)")]
    public List<GameObject> deadEndMaps = new List<GameObject>();

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
    public int minimumGates = 2; // mínimo de openGates para permitir túneles y dead ends

    public List<GameObject> usedMaps = new List<GameObject>();

    // Copias originales para resetear al morir
    public List<GameObject> originalRoomMaps = new List<GameObject>();
    public List<GameObject> originalTunnelMaps = new List<GameObject>();
    public List<GameObject> originalDeadEndMaps = new List<GameObject>();

    public GateBehavior lobbyGate;

    // para evitar dos túneles seguidos
    private bool lastWasTunnel = false;

    private void Start()
    {
        // Guardar copia de los mapas originales para cada lista
        originalRoomMaps   = new List<GameObject>(roomMaps);
        originalTunnelMaps = new List<GameObject>(tunnelMaps);
        originalDeadEndMaps = new List<GameObject>(deadEndMaps);
    }

    /// <summary>
    /// Viaja al siguiente mapa. Puede reutilizar uno ya existente (linkedMap) o instanciar uno nuevo desde las listas.
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
        openGates--;

        // Necesitamos que el nombre del mapa contenga la puerta opuesta (L/R/T/B)
        char neededGateLetter = OrientationToLetter(opposite);

        // Comprobar bioma objetivo (si no se especifica, usamos currentBiome)
        BiomeType targetBiome = biomeToGo.HasValue ? biomeToGo.Value : currentBiome;

        // 1) Obtener candidatos de cada lista pública (Rooms / Tunnels / DeadEnds),
        // filtrando por bioma y letra de puerta necesaria.
        List<GameObject> roomCandidates = roomMaps
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

        List<GameObject> tunnelCandidates = tunnelMaps
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

        List<GameObject> deadEndCandidates = deadEndMaps
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

        if (roomCandidates.Count == 0 && tunnelCandidates.Count == 0 && deadEndCandidates.Count == 0)
        {
            Debug.LogWarning("[MapsController] No hay mapas candidatos válidos para el bioma/orientación requeridos (Rooms/Tunnels/DeadEnds).");
            return;
        }

        // 2) Reglas de minimumGates / maximumGates y tipo de sala
        bool canUseTunnelsAndDeadEnds = openGates >= minimumGates;
        bool mustForceDeadEnd = openGates > maxGatesPossible && canUseTunnelsAndDeadEnds;

        GameObject chosenPrefab = null;

        // 2a) Si debemos forzar DeadEnd y hay candidatos, los intentamos primero
        if (mustForceDeadEnd && deadEndCandidates.Count > 0)
        {
            chosenPrefab = deadEndCandidates[Random.Range(0, deadEndCandidates.Count)];
        }
        else
        {
            // 2b) No se deben agregar Tunnels ni DeadEnds si openGates está por debajo de minimumGates
            if (!canUseTunnelsAndDeadEnds)
            {
                if (roomCandidates.Count > 0)
                {
                    chosenPrefab = roomCandidates[Random.Range(0, roomCandidates.Count)];
                }
                else
                {
                    // Fallback extremo: no hay Rooms, usamos cualquier otro para no bloquear
                    List<GameObject> fallback = new List<GameObject>();
                    fallback.AddRange(tunnelCandidates);
                    fallback.AddRange(deadEndCandidates);

                    if (fallback.Count > 0)
                    {
                        Debug.LogWarning("[MapsController] No hay Rooms disponibles por debajo de minimumGates. Usando fallback (Tunnel/DeadEnd).");
                        chosenPrefab = fallback[Random.Range(0, fallback.Count)];
                    }
                }
            }
            else
            {
                // canUseTunnelsAndDeadEnds == true

                if (lastWasTunnel)
                {
                    // 2c) Si el último fue Tunnel, el siguiente debe ser Room o, si superamos máximo, DeadEnd.
                    if (mustForceDeadEnd && deadEndCandidates.Count > 0)
                    {
                        chosenPrefab = deadEndCandidates[Random.Range(0, deadEndCandidates.Count)];
                    }
                    else if (roomCandidates.Count > 0)
                    {
                        chosenPrefab = roomCandidates[Random.Range(0, roomCandidates.Count)];
                    }
                    else if (tunnelCandidates.Count > 0)
                    {
                        // Fallback: no hay Rooms ni DeadEnds válidos, permitimos Tunnel de nuevo para no romper la run.
                        Debug.LogWarning("[MapsController] No hay Rooms ni DeadEnds disponibles tras un Tunnel. Permitiendo Tunnel consecutivo como fallback.");
                        chosenPrefab = tunnelCandidates[Random.Range(0, tunnelCandidates.Count)];
                    }
                }
                else
                {
                    // 2d) Última sala no fue Tunnel. Podemos usar Rooms y Tunnels aleatoriamente.
                    List<GameObject> pool = new List<GameObject>();
                    pool.AddRange(roomCandidates);
                    pool.AddRange(tunnelCandidates);

                    if (pool.Count > 0)
                    {
                        chosenPrefab = pool[Random.Range(0, pool.Count)];
                    }
                    else if (mustForceDeadEnd && deadEndCandidates.Count > 0)
                    {
                        chosenPrefab = deadEndCandidates[Random.Range(0, deadEndCandidates.Count)];
                    }
                    else if (deadEndCandidates.Count > 0)
                    {
                        // Si no hay Rooms/Tunnels pero sí DeadEnds y estamos por encima de minimumGates, se permite.
                        chosenPrefab = deadEndCandidates[Random.Range(0, deadEndCandidates.Count)];
                    }
                }
            }

            // Si queríamos forzar DeadEnd pero no hay candidatos, dejar constancia
            if (mustForceDeadEnd && deadEndCandidates.Count == 0)
            {
                Debug.LogWarning("[MapsController] Se quería forzar DeadEnd (superado maxGatesPossible) pero no hay candidatos de DeadEnd. Usando otro tipo de sala.");
            }
        }

        if (chosenPrefab == null)
        {
            Debug.LogWarning("[MapsController] No se pudo elegir un prefab de mapa tras aplicar reglas de Rooms/Tunnels/DeadEnds.");
            return;
        }

        // 3) Actualizar openGates sumando (numGatesDelMapa - 1) si no es DeadEnd
        int chosenGateCount = CountGateLettersFromName(chosenPrefab.name);
        bool chosenIsDeadEnd = (chosenGateCount <= 1);
        bool chosenIsTunnel = (chosenGateCount == 2);

        if (!chosenIsDeadEnd)
        {
            // Contamos todas menos la de entrada
            openGates += Mathf.Max(0, chosenGateCount - 1);
        }
        // Si es DeadEnd, no se suma nada (ya restamos 1 al salir)

        // Registrar si el último instanciado fue un túnel
        lastWasTunnel = chosenIsTunnel;

        // 4) Instanciar el mapa y activar variante
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

        // 5) Elegir una Gate válida:
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

        // 6) Linkear las puerta de origen y destino entre sí y sus mapas 
        targetGate.linkedGate = gateFrom;
        targetGate.linkedMap = currentMap;

        gateFrom.GetComponent<GateBehavior>().linkedGate = targetGate.gameObject;
        gateFrom.GetComponent<GateBehavior>().linkedMap = instance;


        // 7) Colocar al jugador en el SpawnPoint de la puerta elegida
        if (player != null && targetGate.spawnPoint != null)
        {
            player.transform.position = targetGate.spawnPoint.position;
        }

        // 8) Encender player y actualizar estado
        if (player != null) player.SetActive(true);

        instance.SetActive(true);
        currentMap = instance;
        currentBiome = selector.biome;

        // 9) Eliminar el prefab elegido de la lista correspondiente (ya forma parte de la run)
        if (roomMaps.Contains(chosenPrefab))
        {
            roomMaps.Remove(chosenPrefab);
        }
        else if (tunnelMaps.Contains(chosenPrefab))
        {
            tunnelMaps.Remove(chosenPrefab);
        }
        else if (deadEndMaps.Contains(chosenPrefab))
        {
            deadEndMaps.Remove(chosenPrefab);
        }

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

    // Estos métodos se siguen usando para el cálculo de openGates
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

        // reset de las listas a sus prefabs originales
        roomMaps      = new List<GameObject>(originalRoomMaps);
        tunnelMaps    = new List<GameObject>(originalTunnelMaps);
        deadEndMaps   = new List<GameObject>(originalDeadEndMaps);

        // reset de random para que la nueva run tenga secuencia distinta
        UnityEngine.Random.InitState(System.Environment.TickCount);

        // Reset estado de enlaces desde el lobby
        lobbyGate.linkedGate = null;
        lobbyGate.linkedMap = null;

        // Reset flag de túneles
        lastWasTunnel = false;

        //destroy every map in usedMaps
        foreach (GameObject map in usedMaps)
        {
            Destroy(map);
        }
        usedMaps.Clear();
    }
}
