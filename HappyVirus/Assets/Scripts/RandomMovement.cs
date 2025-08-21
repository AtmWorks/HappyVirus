//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RandomMovement : MonoBehaviour
//{
//    public Rigidbody2D rb;
//    public float speed = 1;
//    public float accelerationTime = 2f;
//    public float maxSpeed = 5f;
//    private Vector2 movement;
//    private float timeLeft;
//    public float comebackTimer;
//    public bool inSide;
//    public bool ready;
//    public GameObject sourcePoint;

//    void Start()
//    {
//        comebackTimer = 1f;
//        rb = GetComponent<Rigidbody2D>();

//    }

//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        if (collision.tag == "PlqZone" && ready == true)
//        {
//            inSide = true;
//            comebackTimer = 0.5f;
//        }
//    }
//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.tag == "PlqZone")
//        {
//            ready = false;
//            inSide = false;
//        }
//    }
//    void Update()
//    {
//        timeLeft -= Time.deltaTime;
//        if (timeLeft <= 0)
//        {
//            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
//            timeLeft += accelerationTime;
//        }
//    }

//    void FixedUpdate()
//    {
//        if (inSide == true)
//        {
//            Vector2 Movement = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
//            rb.AddForce(Movement);
//            rb.AddForce(movement * maxSpeed);
//        }
//        else if (inSide == false)
//        {
//            comebackTimer -= Time.deltaTime;
//            transform.position = Vector3.MoveTowards(transform.position, sourcePoint.transform.position, 2f * Time.deltaTime);
//        }

//        if (comebackTimer <= 0f)
//        {
//            ready = true;
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movimiento")]
    public float speed = 2f;             // Velocidad base
    public float accelerationTime = 3f;  // Tiempo para cambiar dirección aleatoria
    private Vector2 movement;
    private float timeLeft;

    [Header("Radio de movimiento")]
    public float movementRadius = 4f;    // Radio máximo desde el punto de origen
    private float movementRadiusSqr;

    private Vector3 sourcePosition;      // Posición de spawn/origen

    [Header("Erraticidad")]
    private Vector2 randomForce;
    private float randomForceTimer = 0.5f;
    public float randomForceInterval = 1f; // Frecuencia de cambio de ruido
    public float noiseWeight = 0.1f;         // Peso del ruido sobre la dirección

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sourcePosition = transform.position;       // Guardamos el punto de origen
        movementRadiusSqr = movementRadius * movementRadius;
        timeLeft = accelerationTime;
    }

    void Update()
    {
        // Actualizar dirección aleatoria cada accelerationTime
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            timeLeft += accelerationTime;
        }

        // Actualizar fuerza aleatoria para ruido
        randomForceTimer -= Time.deltaTime;
        if (randomForceTimer <= 0f)
        {
            randomForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            randomForceTimer = randomForceInterval;
        }
    }

    void FixedUpdate()
    {
        Vector2 currentPos = rb.position;
        Vector2 dirToCenter = ((Vector2)sourcePosition - currentPos).normalized;

        // Distancia al centro
        float sqrDistance = ((Vector2)sourcePosition - currentPos).sqrMagnitude;

        Vector2 moveDir;

        if (sqrDistance >= movementRadiusSqr)
        {
            // Si estamos fuera del radio, corregimos fuertemente hacia el centro con un poco de ruido
            moveDir = (dirToCenter * (1f - noiseWeight) + randomForce * noiseWeight).normalized;
        }
        else
        {
            // Movimiento aleatorio dentro del radio
            moveDir = (movement * (1f - noiseWeight) + randomForce * noiseWeight).normalized;
        }

        // Aplicar movimiento con Rigidbody2D
        Vector2 newPos = currentPos + moveDir * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}

