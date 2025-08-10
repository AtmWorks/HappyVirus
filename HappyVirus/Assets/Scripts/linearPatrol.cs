using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MovementType
{
    Linear,
    Lerp,
    Sinusoidal,
}

//public class PatrolMovement : MonoBehaviour
//{
//    public List<GameObject> patrolPoints; // Lista de posiciones a recorrer
//    public float speed = 5f;
//    public float accelerationRate = 2f; // Velocidad de aceleración
//    public MovementType movementType = MovementType.Linear;
//    public bool loopBackwards = false; // Si debe recorrer las posiciones en sentido contrario al llegar al final

//    private Rigidbody2D rb;
//    private Vector2 targetPosition;
//    private int currentPatrolIndex = 0;
//    private float currentSpeed = 0f;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        SetTargetPosition();
//    }

//    private void Update()
//    {
//        // Determinar la dirección hacia el punto objetivo
//        Vector2 direction = (targetPosition - rb.position).normalized;

//        // Calcular la distancia hasta el punto objetivo
//        float distance = Vector2.Distance(rb.position, targetPosition);

//        // Si estamos cerca del objetivo, cambiar al siguiente punto
//        if (distance <= 1f)
//        {
//            IncrementPatrolIndex();
//            SetTargetPosition();
//            currentSpeed = 0f; // Reiniciar la velocidad al cambiar de objetivo
//        }

//        // Calcular la velocidad actual según el tipo de movimiento seleccionado
//        switch (movementType)
//        {
//            case MovementType.Linear:
//                UpdateLinearMovement();
//                break;
//            case MovementType.Lerp:
//                UpdateLerpMovement();
//                break;
//            case MovementType.Sinusoidal:
//                UpdateSinusoidalMovement();
//                break;
//        }

//        // Aplicar la velocidad al Rigidbody2D en la dirección calculada
//        rb.velocity = direction * currentSpeed;
//    }

//    private void SetTargetPosition()
//    {
//        targetPosition = patrolPoints[currentPatrolIndex].transform.position;
//    }

//    private void IncrementPatrolIndex()
//    {
//        if (loopBackwards)
//        {
//            // Recorrer las posiciones en sentido contrario al llegar al final
//            currentPatrolIndex = (currentPatrolIndex == 0) ? patrolPoints.Count - 1 : currentPatrolIndex - 1;
//        }
//        else
//        {
//            // Volver al inicio al llegar al final
//            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
//        }
//    }

//    private void UpdateLinearMovement()
//    {
//        // Movimiento lineal: la velocidad se mantiene constante
//        currentSpeed = speed;
//    }

//    private void UpdateLerpMovement()
//    {
//        // Movimiento mediante Lerp: la velocidad aumenta gradualmente
//        currentSpeed = Mathf.MoveTowards(currentSpeed, speed, accelerationRate * Time.deltaTime);
//    }

//    private void UpdateSinusoidalMovement()
//    {
//        // Movimiento sinusoidal: la velocidad varía sinusoidalmente
//        float amplitude = speed / 2f; // Amplitud de la onda sinusoidal
//        currentSpeed = speed + amplitude * Mathf.Sin(Time.time * 2f); // Frecuencia de la onda
//    }
//}


public class PatrolMovement : MonoBehaviour
{
    public List<GameObject> patrolPoints; // Lista de posiciones a recorrer
    public float speed = 5f;
    public float accelerationRate = 2f; // Velocidad de aceleración
    public MovementType movementType = MovementType.Linear;
    public bool loopBackwards = false; // Si debe recorrer las posiciones en sentido contrario al llegar al final
    public bool reverseLoop = false; // Si debe invertir el orden al recorrer la lista al llegar al final

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private int currentPatrolIndex = 0;
    private int patrolDirection = 1; // Dirección del patrullaje (1 para adelante, -1 para atrás)
    private float currentSpeed = 0f;
    public float oscillationValue = 0f;
    public float waitingTime = 2f; // Tiempo de espera en segundos
    private bool isWaiting = false; // Variable para controlar si estamos esperando
    private float waitTimer = 0f; // Temporizador para contar el tiempo de espera

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetTargetPosition();
    }

    private void Update()
    {
        // Determinar la dirección hacia el punto objetivo
        Vector2 direction = (targetPosition - rb.position).normalized;

        // Calcular la distancia hasta el punto objetivo
        float distance = Vector2.Distance(rb.position, targetPosition);

        // Si estamos cerca del objetivo y no estamos esperando, cambiar al siguiente punto
        if (distance <= 1f && !isWaiting)
        {
            isWaiting = true; // Activar el estado de espera
            waitTimer = 0f; // Reiniciar el temporizador de espera
        }

        // Si estamos esperando, contar el tiempo de espera
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;

            // Si el tiempo de espera ha terminado, avanzar al siguiente punto
            if (waitTimer >= waitingTime)
            {
                isWaiting = false; // Desactivar el estado de espera
                IncrementPatrolIndex(); // Cambiar al siguiente punto
                SetTargetPosition();
                currentSpeed = 0f; // Reiniciar la velocidad al cambiar de objetivo
            }
            else
            {
                // Detener el movimiento mientras esperamos
                rb.velocity = Vector2.zero;
                return; // Salir del método sin aplicar la velocidad
            }
        }

        // Calcular la velocidad actual según el tipo de movimiento seleccionado
        switch (movementType)
        {
            case MovementType.Linear:
                UpdateLinearMovement();
                break;
            case MovementType.Lerp:
                UpdateLerpMovement();
                break;
            case MovementType.Sinusoidal:
                UpdateSinusoidalMovement();
                break;
        }

        // Aplicar la velocidad al Rigidbody2D en la dirección calculada
        rb.velocity = direction * currentSpeed;
    }


    private void SetTargetPosition()
    {
        targetPosition = patrolPoints[currentPatrolIndex].transform.position;
    }

    private void IncrementPatrolIndex()
    {
        // Determinar el siguiente índice de patrullaje
        if (reverseLoop && (currentPatrolIndex == 0 || currentPatrolIndex == patrolPoints.Count - 1))
        {
            // Invertir dirección al llegar al inicio o al final
            patrolDirection *= -1;
        }

        // Incrementar el índice de patrullaje
        currentPatrolIndex += patrolDirection;

        // Revisar los límites del índice de patrullaje
        if (currentPatrolIndex < 0)
        {
            currentPatrolIndex = patrolPoints.Count - 1;
        }
        else if (currentPatrolIndex >= patrolPoints.Count)
        {
            currentPatrolIndex = 0;
        }
    }

    private void UpdateLinearMovement()
    {
        // Movimiento lineal: la velocidad se mantiene constante
        currentSpeed = speed;
    }

    private void UpdateLerpMovement()
    {
        // Movimiento mediante Lerp: la velocidad aumenta gradualmente
        currentSpeed = Mathf.MoveTowards(currentSpeed, speed, accelerationRate * Time.deltaTime);
    }

    private void UpdateSinusoidalMovement()
    {
        // Movimiento sinusoidal: la velocidad varía sinusoidalmente
        float amplitude = speed / 2f; // Amplitud de la onda sinusoidal
        currentSpeed = speed + amplitude * Mathf.Sin(Time.time * 2f); // Frecuencia de la onda
    }


}

