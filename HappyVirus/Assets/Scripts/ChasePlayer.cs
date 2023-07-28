using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{

    private Rigidbody2D rb;
    private Transform moveTo;
    public float ChaseSpeed;
    public float maxSpeed;
    public bool isChasing;
    private float speedIncreaseInterval = 1f;
    private float timeSinceLastSpeedIncrease;

   // private Rigidbody2D rb;
    private Quaternion rotacionInicial;

    [SerializeField]
    private float velocidadRotacion = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isChasing = false;
        timeSinceLastSpeedIncrease = 0f;
        rotacionInicial = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Virus" && isChasing == false)
        {
            isChasing = true;
            moveTo = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Virus" && isChasing == true)
        {
            isChasing = false;
        }
    }

    void FixedUpdate()
    {
        if (isChasing == true)
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

        // Obtenemos el ángulo de rotación actual en el eje Z
        float anguloActual = transform.rotation.eulerAngles.z;
        // Si la rotación actual es diferente de la rotación inicial en el eje Z
        if (anguloActual != rotacionInicial.eulerAngles.z)
        {
            // Calculamos el ángulo de rotación necesario para volver a la rotación inicial
            float anguloObjetivo = rotacionInicial.eulerAngles.z;

            // Calculamos la dirección de rotación más corta (en sentido horario o antihorario)
            float rotacion = anguloObjetivo - anguloActual;
            if (rotacion > 180)
                rotacion -= 360;
            else if (rotacion < -180)
                rotacion += 360;

            // Aplicamos una fuerza para rotar el objeto en la dirección correcta
            rb.AddTorque(rotacion * velocidadRotacion);
        }
    }
}
