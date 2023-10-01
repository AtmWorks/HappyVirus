using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public string virusTag = "Virus";
    public float maxChaseSpeed = 3f;
    //public float minChaseTimer = 4f;
    //public float maxChaseTimer = 6f;
    //public float attackTimerDuration = 4f;
    private Quaternion rotacionInicial;
    private float velocidadRotacion = 5f;

    public bool canChase = false;
    //public bool canAttack = false;

    private float chaseSpeed = 1.5f;
    //private float chaseTimer = 0f;
    //private float attackTimer = 0f;

    private Transform virusTransform;
    private Rigidbody2D rb;
    public Animator animatorP;

    private Vector3 playerLastPosition;
    private float lastUpdateTime;

    private float rotationTimer = 0f; // Timer for switching between rotation modes
    private bool rotateClockwise = true; // Indicates the current rotation mode


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       // animatorP = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(virusTag))
        {
            virusTransform = other.transform;
            //canChase = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(virusTag))
        {
            virusTransform = other.transform;
            //canChase = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            //Attack();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(virusTag))
        {
            virusTransform = null;
            canChase = false;
            //ResetChase();
        }
    }

    private void FixedUpdate()
    {
        if (canChase)
        {
           Chase();
        }
        if (virusTransform != null)
        {
            float distance = Vector2.Distance(transform.position, virusTransform.position);

            // Si la distancia es menor que cierto valor (por ejemplo, 1 unidad), realiza un ataque
            if (distance < 5f)
            {
                Attack();
            }
        }
        else if (!canChase)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.1f * Time.deltaTime);
        }
        //Para mantener la rotacion firme
        float anguloActual = transform.rotation.eulerAngles.z;
        if (anguloActual != rotacionInicial.eulerAngles.z)
        {
            float anguloObjetivo = rotacionInicial.eulerAngles.z;
            float rotacion = anguloObjetivo - anguloActual;
            if (rotacion > 180)
                rotacion -= 360;
            else if (rotacion < -180)
                rotacion += 360;
            rb.AddTorque(rotacion * velocidadRotacion);
        }
    }


    private void Chase()
    {
        if (virusTransform == null)
        {
            return;
        }

        rotationTimer += Time.deltaTime;

        if (rotationTimer >= Random.Range(0.5f, 2f)) // Change rotation mode every 1 second
        {
            rotationTimer = 0f;
            rotateClockwise = !rotateClockwise; // Switch rotation direction
        }

        Vector2 direction = (virusTransform.position - transform.position).normalized;

        // Rotate the direction vector by 15 degrees in either direction based on the mode
        float rotationAmount = rotateClockwise ? 30f : -30f;

        //TODO: direction has to reach rotationAmount Gradually instead this:
        //direction = Quaternion.Euler(0f, 0f, rotationAmount) * direction;
        float rotationSpeed = 20f;
        direction = Vector2.Lerp(direction, Quaternion.Euler(0f, 0f, rotationAmount) * direction, rotationSpeed * Time.deltaTime);
        rb.velocity = direction * chaseSpeed;

        // Increment chase speed gradually up to maxChaseSpeed
        chaseSpeed = Mathf.Min(chaseSpeed + Time.deltaTime, maxChaseSpeed);
    }

    //private void Chase()
    //{
    //    if (virusTransform == null)
    //    {
    //        return;
    //    }


    //    Vector2 direction = (virusTransform.position - transform.position).normalized;
    //    rb.velocity = direction * chaseSpeed;

    //    // Increment chase speed gradually up to maxChaseSpeed
    //    chaseSpeed = Mathf.Min(chaseSpeed + Time.deltaTime, maxChaseSpeed);

    //}

    private void Attack()
    {

        animatorP.SetBool("isAttack", true);
        StartCoroutine(dateUnRespiro());
    }

    IEnumerator dateUnRespiro()
    {
        yield return new WaitForSeconds(1.5f);

        chaseSpeed = chaseSpeed/2;
        animatorP.SetBool("isAttack", false);

    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChasePlayer : MonoBehaviour
//{
//    private Rigidbody2D rb;
//    private Transform moveTo;
//    public float ChaseSpeed;
//    public float maxSpeed;
//    public bool isChasing;
//    public bool isAttack;
//    private float speedIncreaseInterval = 1f;
//    private float timeSinceLastSpeedIncrease;
//    private Quaternion rotacionInicial;
//    public Animator animatorP;
//    public float timer;
//    private Coroutine AttackCoroutine;
//    public bool canAttack;

//    [SerializeField]
//    private float velocidadRotacion = 1f;

//    void Start()
//    {
//        timer = Random.Range(0f, -3f);
//        rb = GetComponent<Rigidbody2D>();
//        canAttack = false;
//        isChasing = false;
//        isAttack = false;
//        timeSinceLastSpeedIncrease = 0f;
//        rotacionInicial = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);

//    }

//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        if (collision.tag == "Virus" && isChasing == false)
//        {
//            isChasing = true;
//            moveTo = collision.transform;
//            canAttack = true;
//        }
//    }

//    private void OnTriggerExit2D(Collider2D col)
//    {
//        if (col.tag == "Virus" && isChasing == true)
//        {
//            isChasing = false;
//            canAttack = false;
//        }
//    }

//    void FixedUpdate()
//    {
//        timer += Time.deltaTime;
//        if (isChasing == true )
//        {
//            timeSinceLastSpeedIncrease += Time.fixedDeltaTime;
//            if (timeSinceLastSpeedIncrease >= speedIncreaseInterval && ChaseSpeed < maxSpeed)
//            {
//                ChaseSpeed += 0.5f;
//                timeSinceLastSpeedIncrease = 0f;
//            }

//            Vector2 movement = Vector2.MoveTowards(rb.position, moveTo.position, ChaseSpeed * Time.fixedDeltaTime);
//            rb.MovePosition(movement);
//            if (isAttack == false && canAttack ==false)
//            {
//                animatorP.enabled = false;

//            }

//        }
//        if (timer >= 2.2f && isAttack == true)
//        {
//            Attack();
//        }
//        if (timer > 4f)
//        {
//            if (canAttack == true)
//            {
//                isAttack = true;
//                animatorP.enabled = true;
//                animatorP.SetBool("isAttack", true);
//                timer = 0;
//                canAttack = false;
//            }

//        }

//        //mantener la rotacion
//        float anguloActual = transform.rotation.eulerAngles.z;
//        if (anguloActual != rotacionInicial.eulerAngles.z)
//        {
//            float anguloObjetivo = rotacionInicial.eulerAngles.z;
//            float rotacion = anguloObjetivo - anguloActual;
//            if (rotacion > 180)
//                rotacion -= 360;
//            else if (rotacion < -180)
//                rotacion += 360;
//            rb.AddTorque(rotacion * velocidadRotacion);
//        }
//    }
//    //public void dashToVirus() //no funciona muy bien
//    //{
//    //    Vector2 virusPosition = moveTo.position;
//    //    Vector2 direction = (virusPosition - rb.position).normalized;
//    //    float dashForce = 100f;
//    //    rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
//    //    timer = Random.Range(-5f, 0f);
//    //    animatorP.SetBool("isAttack", false);
//    //    isAttack = false;
//    //}

//    public void Attack()
//    {
//        if (AttackCoroutine != null)
//        {
//            StopCoroutine(AttackCoroutine); // Detener la corutina anterior si ya estaba en progreso
//        }
//        AttackCoroutine = StartCoroutine(attackCoroutine());
//        timer = Random.Range(-5f, 0f);
//        animatorP.SetBool("isAttack", false);
//        canAttack= true;
//    }

//    private IEnumerator attackCoroutine()
//    {

//        ChaseSpeed = 0f; // Ajusta la velocidad del dash según sea necesario
//        Debug.Log("ENEMY DASHED");
//        // Establecer la velocidad deseada

//        // Esperar un tiempo determinado para el movimiento del dash
//        float dashDuration = 0.2f; // Ajusta la duración del dash según sea necesario
//        yield return new WaitForSeconds(dashDuration);

//        // Después de la duración del dash, detener el movimiento
//        ChaseSpeed = 1f;



//    }
//}