using UnityEngine;

public class DisableChildOnTrigger : MonoBehaviour
{
    [Header("Modo de funcionamiento")]
    public bool isTriggerHandled = false;      // true = trigger, false = distancia

    [Header("Config distancia")]
    public float desiredDistance = 50f;
    public GameObject Virus;                   // Jugador

    [Header("Config hijo")]
    public GameObject Child;

    [Header("Config Background Element")]
    public bool isBackgroundElement = false;

    // Referencias cacheadas para isBackgroundElement
    private SpriteRenderer spriteRenderer;
    private RotationSlow rotationSlowScript;

    void Start()
    {
        // Buscar el hijo si no está asignado y no es background element
        if (!isBackgroundElement && Child == null && transform.childCount > 0)
            Child = transform.GetChild(0).gameObject;

        // Buscar jugador si no está asignado (solo para modo distancia)
        if (!isTriggerHandled && Virus == null)
            Virus = GameObject.FindWithTag("Player");

        // Cachear componentes si es background element
        if (isBackgroundElement)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rotationSlowScript = GetComponent<RotationSlow>();
            if (spriteRenderer == null)
                Debug.LogWarning($"{name} isBackgroundElement is true but no SpriteRenderer found.");
            if (rotationSlowScript == null)
                Debug.LogWarning($"{name} isBackgroundElement is true but no RotationSlow script found.");
        }
    }

    void Update()
    {
        if (isTriggerHandled)
        {
            // No hacemos nada aquí, usamos triggers
            return;
        }

        if (Virus == null)
        {
            Virus = GameObject.FindWithTag("Player");
            if (Virus == null) return;
        }

        float distance = Vector3.Distance(Virus.transform.position, transform.position);

        if (isBackgroundElement)
        {
            // Activar/desactivar componentes en este objeto
            bool shouldBeActive = distance <= desiredDistance;
            if (spriteRenderer != null)
                spriteRenderer.enabled = shouldBeActive;
            if (rotationSlowScript != null)
                rotationSlowScript.enabled = shouldBeActive;
        }
        else
        {
            // Activar/desactivar hijo
            if (Child != null)
            {
                Child.SetActive(distance <= desiredDistance);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // --- Modo Trigger ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggerHandled) return;
        if (!other.CompareTag("Player")) return;

        if (isBackgroundElement)
        {
            if (spriteRenderer != null) spriteRenderer.enabled = true;
            if (rotationSlowScript != null) rotationSlowScript.enabled = true;
        }
        else
        {
            if (Child != null) Child.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isTriggerHandled) return;
        if (!other.CompareTag("Player")) return;

        if (isBackgroundElement)
        {
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (rotationSlowScript != null) rotationSlowScript.enabled = false;
        }
        else
        {
            if (Child != null) Child.SetActive(false);
        }
    }
}
