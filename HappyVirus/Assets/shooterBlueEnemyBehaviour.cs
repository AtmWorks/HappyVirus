

//THE BEST SO FAR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterBlueEnemyBehaviour : MonoBehaviour
{
    public string virusTag = "Virus";
    public float safeDistanceMin;
    public float safeDistanceMax;
    public float moveSpeed;
    public float startSpeed;
    public float sinusSpeed;
    public float rotationSpeed = 5.0f;
    public float yOffset = 1.0f;

    public Animator animator;

    public bool isY;
    private Transform virusTransform;
    private bool isMovingSinusoidal = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startSpeed = moveSpeed;
        animator = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {

    float rotationZ = this.gameObject.transform.rotation.eulerAngles.z;
    
    //TODO: Si el objeto virus esta a mi derecha, mi scale.y sera negativa. Si esta a mi izquierda sera positiva
   

        ////////////////////////
     if ((rotationZ >= 315 || rotationZ <= 45) || (rotationZ >= 135 && rotationZ <= 225))
    {
        isY = true;
    }
    else
    {
        isY = false;
    }
        if (virusTransform == null)
        {
            Debug.Log("There is no virus");
            return;
        }

        Vector2 virusDirection = virusTransform.position - transform.position;
        float distanceToVirus = virusDirection.magnitude;


        //if (virusTransform != null)
        //{
        //    float angleToVirus = Mathf.Atan2(virusDirection.y, virusDirection.x) * Mathf.Rad2Deg;

        //    // Compara el ángulo entre el enemigo y el virus para determinar la escala en Y
        //    if (angleToVirus > 90f || angleToVirus < -90f)
        //    {
        //        // Si el virus está a la derecha del enemigo, establece la escala en Y como negativa.
        //        Vector3 newScale = transform.localScale;
        //        newScale.y = -Mathf.Abs(newScale.y);
        //        transform.localScale = newScale;
        //    }
        //    else
        //    {
        //        // Si el virus está a la izquierda del enemigo, establece la escala en Y como positiva.
        //        Vector3 newScale = transform.localScale;
        //        newScale.y = Mathf.Abs(newScale.y);
        //        transform.localScale = newScale;
        //    }
        //}
        if (virusTransform != null)
        {
            float angleToVirus = Mathf.Atan2(virusDirection.y, virusDirection.x) * Mathf.Rad2Deg;

            // Compara el ángulo entre el enemigo y el virus para determinar la escala en Y
            if (angleToVirus > 90f || angleToVirus < -90f)
            {
                // Si el virus está a la derecha del enemigo, establece la escala en Y como negativa.
                //animator.SetBool("isRight", true);
                
            }
            else
            {
                // Si el virus está a la izquierda del enemigo, establece la escala en Y como positiva.
                //animator.SetBool("isRight", false);


            }
        }

        // Rotate towards the virus
        float angle = Mathf.Atan2(virusDirection.y, virusDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (distanceToVirus > safeDistanceMax)
        {
            isMovingSinusoidal = false;

            // Move towards the virus if too far away
            moveSpeed = startSpeed;
            
        }
        else if (distanceToVirus < safeDistanceMin)
        {
            isMovingSinusoidal = false;

            moveSpeed = -startSpeed;

            // Move away from the virus if too close
        }

        SmoothMoveTowards(virusTransform.position, moveSpeed);

        if ((distanceToVirus < safeDistanceMax) && (distanceToVirus > safeDistanceMin))
        {
            moveSpeed = 0;
        }

        if (isMovingSinusoidal)
            {
                StartCoroutine(MoveSinusoidal());

            }


        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            virusTransform = collision.gameObject.transform;
        }
    }
    private void SmoothMoveTowards(Vector2 targetPosition, float speed)
    {
        //isMovingSinusoidal = false;

        Vector2 direction = (targetPosition - rb.position).normalized;
        Vector2 targetVelocity = direction * speed;

        // Interpola la velocidad actual hacia la velocidad objetivo
        rb.velocity = Vector2.Lerp(rb.velocity*1.1f, targetVelocity, Time.deltaTime);
    }

    IEnumerator MoveSinusoidal()
    {
       if (isY)
        {
            isMovingSinusoidal = true;
            float initialY = transform.localPosition.y; // Usar transform.localPosition.y para el eje local Y
            float time = 0.0f;

            while (isMovingSinusoidal)
            {
                time += Time.deltaTime;
                float newY = initialY + Mathf.Sin(time * sinusSpeed) * yOffset;
                Vector3 localPosition = transform.localPosition; // Obtener la posición local actual
                localPosition.y = newY; // Actualizar solo el componente Y en el espacio local
                transform.localPosition = localPosition; // Asignar la nueva posición local
                yield return null;
            }
        }
        if (!isY) 
        {
            isMovingSinusoidal = true;
            float initialX = transform.localPosition.x; // Usar transform.localPosition.y para el eje local Y
            float time = 0.0f;

            while (isMovingSinusoidal)
            {
                time += Time.deltaTime;
                float newX = initialX + Mathf.Sin(time * sinusSpeed) * yOffset;
                Vector3 localPosition = transform.localPosition; // Obtener la posición local actual
                localPosition.x = newX; // Actualizar solo el componente Y en el espacio local
                transform.localPosition = localPosition; // Asignar la nueva posición local
                yield return null;
            }
        }
    }
}

//THE BEST SO FAR
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class shooterBlueEnemyBehaviour : MonoBehaviour
//{
//    public string virusTag = "Virus";
//    public float safeDistanceMin;
//    public float safeDistanceMax;
//    public float moveSpeed;
//    public float sinusSpeed;
//    public float rotationSpeed = 5.0f;
//    public float yOffset = 1.0f;

//    private Transform virusTransform;
//    private bool isMovingSinusoidal = false;

//    void Start()
//    {

//    }

//    void Update()
//    {
//        if (virusTransform == null)
//        {
//            Debug.Log("There is no virus");
//            return;
//        }

//        Vector2 virusDirection = virusTransform.position - transform.position;
//        float distanceToVirus = virusDirection.magnitude;

//        // Rotate towards the virus
//        float angle = Mathf.Atan2(virusDirection.y, virusDirection.x) * Mathf.Rad2Deg;
//        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
//        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

//        if (distanceToVirus > safeDistanceMax)
//        {
//            // Move towards the virus if too far away
//            transform.position = Vector2.MoveTowards(transform.position, virusTransform.position, moveSpeed * Time.deltaTime);
//            isMovingSinusoidal = false;
//        }
//        else if (distanceToVirus < safeDistanceMin)
//        {
//            // Move away from the virus if too close
//            transform.position = Vector2.MoveTowards(transform.position, virusTransform.position, -moveSpeed * Time.deltaTime);
//            isMovingSinusoidal = false;
//        }
//        else
//        {
//            // Move sinusoidally within safe range
//            if (!isMovingSinusoidal)
//            {
//                StartCoroutine(MoveSinusoidal());
//            }
//        }
//    }

//    public void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.tag == "Virus")
//        {
//            virusTransform = collision.gameObject.transform;
//        }
//    }
//    IEnumerator MoveSinusoidal()
//    {
//        isMovingSinusoidal = true;
//        float initialY = transform.localPosition.y; // Usar transform.localPosition.y para el eje local Y
//        float time = 0.0f;

//        while (isMovingSinusoidal)
//        {
//            time += Time.deltaTime;
//            float newY = initialY + Mathf.Sin(time * sinusSpeed) * yOffset;
//            Vector3 localPosition = transform.localPosition; // Obtener la posición local actual
//            localPosition.y = newY; // Actualizar solo el componente Y en el espacio local
//            transform.localPosition = localPosition; // Asignar la nueva posición local
//            yield return null;
//        }
//    }
//}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class shooterBlueEnemyBehaviour : MonoBehaviour
//{
//    public string virusTag = "Virus"; // El tag del objeto Virus.
//    public float velocidadMovimiento = 5f; // Velocidad de movimiento del objeto.
//    public float rangoObjetivo = 5f; // Rango al que el objeto se acercará.

//    private Transform objetivo; // El objeto Virus.
//    private Rigidbody2D rb2d;
//    private float tiempoInicio;

//    private void Start()
//    {
//        rb2d = GetComponent<Rigidbody2D>();
//        tiempoInicio = Time.time;
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag(virusTag))
//        {
//            objetivo = collision.transform;
//        }
//    }

//    private void Update()
//    {
//        if (objetivo != null)
//        {
//            // Calculamos la dirección hacia el objeto Virus.
//            Vector2 direccion = (objetivo.position - transform.position).normalized;

//            // Calculamos el movimiento senoidal suave.
//            float t = (Time.time - tiempoInicio) * velocidadMovimiento;
//            float seno = Mathf.Sin(t);

//            // Movemos el objeto en la dirección y el movimiento senoidal.
//            rb2d.velocity = direccion * seno;

//            // Rotamos hacia el objeto Virus.
//            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
//            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo));

//            // Si estamos dentro del rango objetivo, dejamos de movernos.
//            if (Vector2.Distance(transform.position, objetivo.position) <= rangoObjetivo)
//            {
//                rb2d.velocity = Vector2.zero;
//            }
//        }
//    }
//}
