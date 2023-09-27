using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class meatBall : MonoBehaviour
{

    private Rigidbody2D rb;
    private Transform moveTo;
    public float ChaseSpeed;
    public float maxSpeed;
    public bool isChasing;
    public bool isAttack;
    private float speedIncreaseInterval = 0.5f;
    private float timeSinceLastSpeedIncrease;
    private Quaternion rotacionInicial;
    public Animator animatorP;
    public float timer;
    private Coroutine dashCoroutine;
    public bool canAttack;

    [SerializeField]
    private float velocidadRotacion = 1f;

    void Start()
    {
        timer = Random.Range(0f, -3f);
        rb = GetComponent<Rigidbody2D>();
        canAttack = false;
        isChasing = false;
        isAttack = false;
        timeSinceLastSpeedIncrease = 0f;
        rotacionInicial = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Virus" && isChasing == false)
        {
            isChasing = true;
            moveTo = collision.transform;
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Virus" && isChasing == true)
        {
            isChasing = false;
            canAttack = false;
        }
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (isChasing == true )
        {
            timeSinceLastSpeedIncrease += Time.fixedDeltaTime;
            if (timeSinceLastSpeedIncrease >= speedIncreaseInterval && ChaseSpeed < maxSpeed)
            {
                ChaseSpeed += 0.5f;
                timeSinceLastSpeedIncrease = 0f;
            }

                Vector2 movement = Vector2.MoveTowards(rb.position, moveTo.position, ChaseSpeed * Time.fixedDeltaTime);
                rb.MovePosition(movement);

        }
        if (timer >= 3.3f && isAttack == true )
        {
            dashToVirus();
        }
        if (timer > 4f)
        {
            if (canAttack == true)
            {
                isAttack = true;
                animatorP.SetBool("isAttack", true);
                timer = 0;
            }
            
        }

        //mantener la rotacion
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
    //public void dashToVirus() //no funciona muy bien
    //{
    //    Vector2 virusPosition = moveTo.position;
    //    Vector2 direction = (virusPosition - rb.position).normalized;
    //    float dashForce = 100f;
    //    rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
    //    timer = Random.Range(-5f, 0f);
    //    animatorP.SetBool("isAttack", false);
    //    isAttack = false;
    //}

    public void dashToVirus()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine); // Detener la corutina anterior si ya estaba en progreso
        }
        dashCoroutine = StartCoroutine(DashCoroutine());
        timer = Random.Range(-5f, 0f);
        animatorP.SetBool("isAttack", false);
        isAttack = false;
    }

    private IEnumerator DashCoroutine()
    {

        ChaseSpeed = 20f; // Ajusta la velocidad del dash según sea necesario
        Debug.Log("ENEMY DASHED");
        // Establecer la velocidad deseada

        // Esperar un tiempo determinado para el movimiento del dash
        float dashDuration = 0.2f; // Ajusta la duración del dash según sea necesario
        yield return new WaitForSeconds(dashDuration);

        // Después de la duración del dash, detener el movimiento
        ChaseSpeed=1f;
    }
}
