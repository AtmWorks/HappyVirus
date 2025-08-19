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
    public Rigidbody2D rb;
    public float speed = 1f;
    public float accelerationTime = 2f;
    public float maxSpeed = 5f;
    private Vector2 movement;
    private float timeLeft;
    public float comebackTimer=0f;
    public bool inSide;
    public bool ready;
    public GameObject sourcePoint;

    public float detectionRadius = 0.5f; // radio de detección

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Comprobar distancia cada 0.5 segundos
        InvokeRepeating("CheckDistance", 0f, 0.5f);
    }

    void CheckDistance()
    {
        // Distancia al cuadrado para optimizar
        float sqrDistance = (transform.position - sourcePoint.transform.position).sqrMagnitude;
        inSide = sqrDistance <= detectionRadius * detectionRadius;

        // Actualizar comebackTimer
        if (!inSide && comebackTimer > 0f)
        {
            comebackTimer -= 0.5f;
        }
        else if (comebackTimer <= 0f)
        {
            ready = true;
        }
    }

    void Update()
    {
        // Movimiento aleatorio cada accelerationTime
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            timeLeft += accelerationTime;
        }
    }

    void FixedUpdate()
    {
        if (inSide && ready)
        {
            Vector2 randomForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.AddForce(randomForce);
            rb.AddForce(movement * maxSpeed);
        }
        else if (!inSide)
        {
            // Volver al sourcePoint
            transform.position = Vector3.MoveTowards(transform.position, sourcePoint.transform.position, 2f * Time.deltaTime);
        }
    }
}
