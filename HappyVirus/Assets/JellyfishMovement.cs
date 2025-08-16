
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JellyfishMovement : MonoBehaviour
{
    [Header("Referencia")]
    public GameObject anchor;

    [Header("Rangos de movimiento")]
    public float maxDistance = 5f;

    [Header("Velocidades de desplazamiento")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    [Header("Velocidades de rotación (grados/seg)")]
    public float minRotationSpeed = 10f;
    public float maxRotationSpeed = 40f;

    [Header("Tiempo entre movimientos")]
    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;

    [Header("Control de Animator")]
    public Animator animator;
    public bool isReadyToMove = false;
    public bool showGizmos = true;

    // Privadas
    private Rigidbody2D rb;
    private bool isMoving = false;
    private float idleTimer;
    private bool returnToAnchor = false;
    private Quaternion targetRotation;
    private float currentSpeed;
    private float currentRotationSpeed;
    private Vector2 returnTargetPoint;

    public bool isAvoiding;

    private void Start()
    {
        isAvoiding = false;
        rb = GetComponent<Rigidbody2D>();

        if (!anchor)
        {
            Debug.LogError("++++El anchor no está asignado en el inspector.");
            enabled = false;
            return;
        }

        EnterIdleState();
    }

    private void Update()
    {
        HandleRotation();

        if (!isMoving)
        {
            HandleIdleTimer();
        }
        else
        {
            // 🔹 Desaceleración suave de movimiento y rotación
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime);
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, Time.deltaTime);

            rb.velocity = transform.up * currentSpeed;

            if (currentSpeed < 0.05f && Mathf.Abs(currentRotationSpeed) < 0.05f)
            {
                EnterIdleState();
            }
        }
    }

    private void EnterIdleState()
    {
        animator.SetBool("isIdle", true);
        isMoving = false;
        isReadyToMove = false;
        rb.velocity = Vector2.zero;
        if (isAvoiding){ rb.mass = 0.2f; } else { rb.mass = 25f; }

        idleTimer = isAvoiding ? (Random.Range(minIdleTime, maxIdleTime))*2 : Random.Range(minIdleTime, maxIdleTime);

        float distFromAnchor = Vector2.Distance(transform.position, anchor.transform.position);
        if (distFromAnchor > maxDistance)
        {
            returnToAnchor = true;

            float safeRadius = maxDistance / 6f;
            Vector2 dirToAnchor = ((Vector2)anchor.transform.position - (Vector2)transform.position).normalized;
            returnTargetPoint = (Vector2)anchor.transform.position - dirToAnchor * safeRadius; 

            Vector2 dirToTarget = (returnTargetPoint - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
            targetRotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            returnToAnchor = false;
        }

        // 🔹 Velocidad angular aleatoria
        currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed) *
                               Mathf.Sign(Random.value - 0.5f);
    }

    private void HandleRotation()
    {
        if (returnToAnchor && !isMoving && !isAvoiding)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                Mathf.Abs(currentRotationSpeed) * Time.deltaTime
            );
        }
        else if (isAvoiding)
        {
            // Encuentra un objeto en la escena con el tag "Player"
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // Vector desde la medusa al player
                Vector2 dirToPlayer = (player.transform.position - transform.position).normalized;

                // Invertir la dirección para mirar en sentido opuesto
                Vector2 oppositeDir = -dirToPlayer;

                // Calcular el ángulo y aplicar rotación
                float angle = Mathf.Atan2(oppositeDir.y, oppositeDir.x) * Mathf.Rad2Deg - 90f;
                Quaternion targetRot = Quaternion.Euler(0, 0, angle);

                // Rotación x3
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    Mathf.Abs(currentRotationSpeed * 3f) * Time.deltaTime
                );
            }
        }
        else
        {
            transform.Rotate(Vector3.forward * currentRotationSpeed * Time.deltaTime);
        }
    }

    private void HandleIdleTimer()
    {
        if (idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;
        }
        else
        {
            if (returnToAnchor && !isAvoiding)
            {
                float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);
                if (angleDiff > 1f)
                    return;
            }

            animator.SetBool("isIdle", false);
        }

        if (isReadyToMove)
        {
            StartMovement();
        }
    }

    private void StartMovement()
    {
        isMoving = true;
        isReadyToMove = false;

        currentSpeed = Random.Range(minSpeed, maxSpeed);
        rb.velocity = transform.up * currentSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isMoving) return;
        if (collision.gameObject.tag == "Virus") return;
        // 🔹 Frenar rápidamente
        currentSpeed *= 0.2f; // pierde 80% de velocidad
        currentRotationSpeed *= 0.2f; // pierde 80% de rotación
        
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        if (anchor != null)
        {
            Gizmos.color = Color.magenta;
            DrawWireCircle(anchor.transform.position, maxDistance, 32);

            Gizmos.color = Color.cyan;
            DrawWireCircle(anchor.transform.position, maxDistance / 3f, 32);

            if (returnToAnchor)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(returnTargetPoint, 0.1f);
            }
        }

        Gizmos.color = Color.yellow;
        DrawWireCircle(transform.position, 0.2f, 16);
    }

    private void DrawWireCircle(Vector2 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + Vector2.right * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}
