using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Componente del mapa. Gestiona un catálogo de variantes ya presentes en el prefab
/// (todas desactivadas de inicio) y activa UNA según la lógica indicada.
/// Tras activar, detecta los hijos directos "SPAWN_" y rellena 'gates'.
/// </summary>
public class MapSelector : MonoBehaviour
{
    [Header("Gates activas del mapa (se rellenan al activar variante)")]
    public GameObject[] gates;

    [Header("Datos del mapa")]
    public BiomeType biome = BiomeType.Biome1;

    /// <summary>
    /// Entrada de catálogo: combinación de (variant, difficulty, isTransition)
    /// con una lista de GameObjects (ya en el prefab) que pueden activarse.
    /// </summary>
    [System.Serializable]
    public class VariantEntry
    {
        [Header("Clave de la variante")]
        public string id = "";                        // Opcional: para debug/guardado
        public VariantType variant = VariantType.Default;
        [Min(1)] public int difficulty = 1;
        public bool isTransition = false;

        [Header("Objetos de esta variante (ya en el prefab, se activan/desactivan)")]
        public List<GameObject> variantObjects = new List<GameObject>();
    }

    [Header("Catálogo de variantes")]
    [Tooltip("Cada entrada = (variant, difficulty, isTransition) con varias opciones de GameObjects posibles.")]
    public List<VariantEntry> variantCatalog = new List<VariantEntry>();

    /// <summary>
    /// Selecciona una lista por (variant, difficulty, hasTransition) con degradación:
    /// - variant exacto o Default si no existe.
    /// - difficulty exacta o restando hasta 1.
    /// - hasTransition debe coincidir; si no es posible, usa Default con isTransition == false.
    /// Activa un objeto al azar de la lista escogida (solo SetActive), busca sus hijos directos "SPAWN_",
    /// rellena 'gates' y devuelve esa lista.
    /// </summary>
    public List<GameObject> variantSelector(VariantType variant, int difficulty, bool hasTransition)
    {
        // 1) Elegir base por tipo: entradas del tipo solicitado; si no hay, Default
        var baseSet = variantCatalog.Where(e => e.variant == variant).ToList();
        if (baseSet.Count == 0)
            baseSet = variantCatalog.Where(e => e.variant == VariantType.Default).ToList();

        if (baseSet.Count == 0)
            return ReturnEmptyWithWarning($"No hay entradas para {variant} ni Default en {name}.");

        // 2) Ajustar dificultad: exacta o decreciendo hasta 1
        var diff = Mathf.Max(1, difficulty);
        List<VariantEntry> selectedByDifficulty = null;

        while (diff >= 1 && (selectedByDifficulty == null || selectedByDifficulty.Count == 0))
        {
            selectedByDifficulty = baseSet.Where(e => e.difficulty == diff).ToList();
            diff--;
        }

        if (selectedByDifficulty == null || selectedByDifficulty.Count == 0)
            return ReturnEmptyWithWarning($"No hay dificultad válida en {name} para el tipo seleccionado.");

        // 3) Coincidencia de transición: si no hay coincidencia, caer a Default con isTransition == false
        var selectedByTransition = selectedByDifficulty.Where(e => e.isTransition == hasTransition).ToList();

        if (selectedByTransition.Count == 0)
        {
            selectedByTransition = variantCatalog
                .Where(e => e.variant == VariantType.Default && e.isTransition == false)
                .ToList();

            if (selectedByTransition.Count == 0)
                return ReturnEmptyWithWarning($"No hay combinación viable de transición en {name}.");
        }

        // 4) Elegir un GameObject al azar dentro de la lista resultante
        var allCandidates = selectedByTransition.SelectMany(e => e.variantObjects).Where(go => go != null).ToList();
        if (allCandidates.Count == 0)
            return ReturnEmptyWithWarning($"No hay GameObjects candidatos en la entrada seleccionada de {name}.");

        var chosen = allCandidates[Random.Range(0, allCandidates.Count)];

        // 5) Activar solo el elegido (no se instancia nada)
        DeactivateAll();
        chosen.SetActive(true);

        // 6) Buscar hijos directos "SPAWN_" del elegido, rellenar 'gates' y devolverlos
        var spawnChildren = GetDirectSpawnChildren(chosen);
        gates = spawnChildren.ToArray();
        
        return spawnChildren;
    }

    // ----------------- Helpers privados (simples y legibles) -----------------

    /// <summary>
    /// Desactiva todos los GameObjects de todas las entradas del catálogo.
    /// </summary>
    private void DeactivateAll()
    {
        foreach (var entry in variantCatalog)
            foreach (var go in entry.variantObjects)
                if (go != null && go.activeSelf)
                    go.SetActive(false);
    }

    /// <summary>
    /// Devuelve los hijos directos de 'root' cuyo nombre empieza por "SPAWN_".
    /// </summary>
    private List<GameObject> GetDirectSpawnChildren(GameObject root)
    {
        var result = new List<GameObject>();
        var t = root.transform;
        for (int i = 0; i < t.childCount; i++)
        {
            var child = t.GetChild(i).gameObject;
            if (child.name.StartsWith("SPAWN_"))
                result.Add(child);
        }
        return result;
    }

    /// <summary>
    /// Utilidad para registrar advertencia y devolver lista vacía dejando 'gates' vacías.
    /// </summary>
    private List<GameObject> ReturnEmptyWithWarning(string msg)
    {
        Debug.LogWarning($"[MapBehavior] {msg}");
        gates = new GameObject[0];
        return new List<GameObject>();
    }
}
