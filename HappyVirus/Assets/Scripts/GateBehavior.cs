using UnityEngine;

public class GateBehavior : MonoBehaviour
{
    [Header("Conexión de mapas")]
    public GameObject linkedMap;     // Mapa al que conecta esta puerta
    public GameObject linkedGate;    // Gate remota a la que conecta esta puerta

    [Header("Orientación")]
    public GateOrientation gateOrientation;   // L, R, T, B

    [Header("Spawn")]
    public Transform spawnPoint;     // Punto de aparición del jugador

    [Header("Transición de bioma (por puerta)")]
    public bool isTransition = false;          
    public BiomeType transitionTo = BiomeType.Biome1; 

    public MapsController mapsController; // Referencia al controlador de mapas

    // Bloqueo para evitar dobles triggers durante una transición
    private bool isBusy = false;

    private void Awake()
    {
        if (mapsController == null)
        {
#if UNITY_2023_1_OR_NEWER
            mapsController = Object.FindFirstObjectByType<MapsController>();
#else
            mapsController = Object.FindObjectOfType<MapsController>();
#endif
        }
    }

    private void Start()
    {
        if (mapsController == null)
        {
            Debug.LogWarning($"[GateBehavior] MapsController no encontrado en escena ({name}).");
        }
        if (spawnPoint == null)
        {
            Debug.LogWarning($"[GateBehavior] spawnPoint no asignado en {name}.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBusy) return;
        if (!other.CompareTag("Player")) return;
        if (mapsController == null) return;

        isBusy = true;

        // biomeToGo: solo se envía si esta puerta es de transición
        BiomeType? biomeToGo = isTransition ? (BiomeType?)transitionTo : null;

        // fromOrientation = orientación de esta gate; gateFrom = esta gate; linkedMap = el que tenga (puede ser null)
        mapsController.travelToNextMap(
            gateOrientation,
            this.gameObject,
            linkedMap,
            biomeToGo
        );

        // Nota: si quieres rearmar la puerta inmediatamente tras el viaje:
        isBusy = false;
        // En muchos diseños se rearma al volver a entrar desde el otro lado,
        // o se deja así para evitar múltiples llamadas durante el fade.
    }
}
