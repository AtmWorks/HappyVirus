using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Componente del mapa. Gestiona un catálogo de variantes ya presentes en el prefab
/// (todas desactivadas de inicio) y activa UNA según la lógica indicada.
/// Tras activar, detecta los hijos "SPAWN_" dentro de SPAWNCONTAINER y rellena 'gates'.
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
    /// Activa un objeto al azar de la lista escogida (solo SetActive), busca las GATES dentro de SPAWNCONTAINER,
    /// elimina (Destroy) todas las variantes no elegidas, limpia las listas, rellena 'gates' y devuelve esa lista.
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

        // 6) (NUEVO) Destruir todas las variantes no elegidas y limpiar listas
        DestroyAllVariantsExcept(chosen);

        // 7) (NUEVO) Buscar SPAWNCONTAINER en los hijos directos del elegido y tomar sus hijos "SPAWN_"
        var spawnChildren = GetSpawnChildrenFromContainer(chosen);
        gates = spawnChildren.ToArray();

        return spawnChildren;
    }

    // ----------------- Helpers privados -----------------

    /// <summary>
    /// Desactiva todos los GameObjects de todas las entradas del catálogo.
    /// </summary>
    private void DeactivateAll()
    {
        foreach (var entry in variantCatalog)
        {
            foreach (var go in entry.variantObjects)
                if (go != null && go.activeSelf)
                    go.SetActive(false);
        }
    }

    /// <summary>
    /// Destruye (Destroy) todas las variantes del catálogo que no sean 'chosen'
    /// y limpia las listas para evitar referencias nulas o tamaños innecesarios.
    /// </summary>
    private void DestroyAllVariantsExcept(GameObject chosen)
    {
        foreach (var entry in variantCatalog)
        {
            // Si el elegido está en esta entry, conservamos solo ese; los demás se destruyen
            if (entry.variantObjects != null && entry.variantObjects.Count > 0)
            {
                for (int i = entry.variantObjects.Count - 1; i >= 0; i--)
                {
                    var go = entry.variantObjects[i];
                    if (go == null) { entry.variantObjects.RemoveAt(i); continue; }

                    if (go != chosen)
                    {
                        // Destroy es diferido a fin de frame; removemos la referencia para limpiar la lista ya
                        Object.Destroy(go);
                        entry.variantObjects.RemoveAt(i);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Busca un hijo directo llamado "SPAWNCONTAINER" bajo 'root' y devuelve
    /// los hijos de ese contenedor cuyo nombre empiece por "SPAWN_".
    /// </summary>
    private List<GameObject> GetSpawnChildrenFromContainer(GameObject root)
    {
        var result = new List<GameObject>();
        if (root == null) return result;

        // Buscar SOLO entre hijos directos
        Transform container = null;
        var t = root.transform;
        for (int i = 0; i < t.childCount; i++)
        {
            var child = t.GetChild(i);
            if (child.name == "SPAWNCONTAINER")
            {
                container = child;
                break;
            }
        }

        if (container == null)
        {
            Debug.LogWarning($"[MapSelector] No se encontró SPAWNCONTAINER bajo {root.name} en {name}.");
            return result;
        }

        // Iterar SOLO los hijos directos del contenedor
        for (int i = 0; i < container.childCount; i++)
        {
            var spawn = container.GetChild(i).gameObject;
            if (spawn.name.StartsWith("SPAWN_"))
                result.Add(spawn);
        }

        return result;
    }

    /// <summary>
    /// Utilidad para registrar advertencia y devolver lista vacía dejando 'gates' vacías.
    /// </summary>
    private List<GameObject> ReturnEmptyWithWarning(string msg)
    {
        Debug.LogWarning($"[MapSelector] {msg}");
        gates = new GameObject[0];
        return new List<GameObject>();
    }
}
