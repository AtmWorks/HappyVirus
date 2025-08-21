using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikerEnemyBehaviour : MonoBehaviour
{
    public Animator _animator;
    public GameObject source1;
    public GameObject source2;
    public GameObject source3;
    public GameObject source4;
    public GameObject rotationParent;
    public GameObject target;

    public GameObject parent;

    public Rigidbody2D rb;
    //public GameObject projectile;

    public float maxSpeed;
    public float accelerationRate = 8f; 
    private Vector2 targetVelocity; // Velocidad objetivo gradual

    public float desiredDistance = 0;

    public bool isShooting;
    public bool canRotate;
    public bool isVirusInRange;
    public bool alreadyShoot;

    public List<GameObject> vfx = new List<GameObject>();

    private GameObject effectToSpawn;

    public float timer;
    public float resetTime;
    public float nextTime;

    public bool isDead;
    public GameObject explosion;



    private Quaternion rotacionInicial;

    [SerializeField]
    private float velocidadRotacion = 1f;

    void Start()
    {
        isDead = false;
        timer = -3 ;
        resetTime = 2f;
        nextTime = -4f;
        effectToSpawn = vfx[0];
        rotacionInicial = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);
        canRotate = false;
        isShooting = false;
        isVirusInRange = false;
        alreadyShoot = false;

    }

    public void shoot()
    {
        canRotate = false;
        if (target != null)
        {
                alreadyShoot = true;
                SpawnVFX();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            enemyDies();
        }
        timer += Time.deltaTime;

        if (isShooting && !alreadyShoot)
        {
            shoot();
        }
        
        if (timer > 0f && target != null )
        {
            _animator.SetBool("imShooting", true);
        }

        if (timer > 1f )
        {
            _animator.SetBool("imShooting", false);
            alreadyShoot = false;
            canRotate = true;

            timer = Random.Range(-6f, -3f);
           
        }

        if (target != null ) 
        {
            float distance = Vector3.Distance(target.gameObject.transform.position, transform.position);

            if (distance > desiredDistance)
            {
                isVirusInRange = false;
            }
            else if (distance < desiredDistance)
            {
                isVirusInRange = true;
            }
        }

        pullBackMovement();

        keepRotation();

        
    }
    public void enemyDies()
    {
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x+1, this.gameObject.transform.position.y +1, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x-1, this.gameObject.transform.position.y -1, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y +1, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y -1, 0), Quaternion.identity);
        Destroy(parent);
    }

    public void pullBackMovement()
    {

        
        if (isVirusInRange)
        {
            Vector2 direction = (target.gameObject.transform.position - transform.position).normalized;
            targetVelocity = -direction * maxSpeed; // Establecer la nueva velocidad objetivo
        }
        else if (target != null)
        {
            float distance = Vector3.Distance(target.gameObject.transform.position, transform.position);
            if (distance > desiredDistance - 1.5 && distance < desiredDistance + 1.5) { return; }
            Vector2 direction = (target.gameObject.transform.position - transform.position).normalized;
            targetVelocity = direction * maxSpeed; // Establecer la nueva velocidad objetivo
        }

        // Aplicar una interpolación suave de la velocidad actual hacia la velocidad objetivo
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
    }


    public void keepRotation()
    {
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
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            target = collision.gameObject;
        }
    }
 

    void SpawnVFX()
    {
        GameObject vfx;
        Quaternion rotationfix = Quaternion.Euler(0f, 0f, 90f);
        vfx = Instantiate(effectToSpawn, source1.transform.position, source1.transform.rotation * rotationfix); 
        vfx = Instantiate(effectToSpawn, source2.transform.position, source2.transform.rotation * rotationfix); 
        vfx = Instantiate(effectToSpawn, source3.transform.position, source3.transform.rotation * rotationfix); 
        vfx = Instantiate(effectToSpawn, source4.transform.position, source4.transform.rotation * rotationfix); 
    }
}
