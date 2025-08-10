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

    void Start()
    {
        // Buscar el hijo si no está asignado
        if (Child == null && transform.childCount > 0)
            Child = transform.GetChild(0).gameObject;

        // Buscar jugador si no está asignado (solo para modo distancia)
        if (!isTriggerHandled && Virus == null)
            Virus = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        // Solo ejecuta la lógica de distancia si no usamos trigger
        if (!isTriggerHandled && Child != null && Virus != null)
        {
            float distance = Vector3.Distance(Virus.transform.position, Child.transform.position);

            if (distance > desiredDistance)
                Child.SetActive(false);
            else
                Child.SetActive(true);
        }
        else if (Child == null)
        {
            Destroy(gameObject);
        }
    }

    // --- Modo Trigger ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggerHandled && other.CompareTag("Player"))
        {
            if (Child != null) Child.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isTriggerHandled && other.CompareTag("Player"))
        {
            if (Child != null) Child.SetActive(false);
        }
    }
}