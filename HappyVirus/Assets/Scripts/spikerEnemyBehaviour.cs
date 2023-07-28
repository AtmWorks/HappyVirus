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

    public Rigidbody2D rb;
    //public GameObject projectile;

    public float speed;
    public float minimumDistance;

    public bool retreat;

    public List<GameObject> vfx = new List<GameObject>();

    private GameObject effectToSpawn;

    public float timer;
    public float resetTime;
    public float nextTime;

    public bool canAttack;

    private Quaternion rotacionInicial;

    [SerializeField]
    private float velocidadRotacion = 1f;

    void Start()
    {
     //   rb = GetComponent<Rigidbody2D>();
        retreat = false;
        timer = Random.Range(-10f, -5f); ;
        resetTime = 2f;
        nextTime = 0f;
        effectToSpawn = vfx[0];
        speed = 3;
        rotacionInicial = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //triger animetion
        //wait 2 secs
        timer += Time.deltaTime;
        if (timer > 0f)
        {
            _animator.SetBool("imShooting", true);

        }
        if (timer > 2f)
        {
            rotationParent.GetComponent<Rotation>().enabled = false; // Desactivar el script

        }
        if (timer > 2.25f)
        {
            _animator.SetBool("imShooting", false);
            SpawnVFX();
            //   rotationParent.GetComponent<Rotation>().enabled = true; // Desactivar el script
            timer = -6f;
        }
        if (timer > -5f && timer < 1.8f)
        {
            rotationParent.GetComponent<Rotation>().enabled = true; // Desactivar el script
            if (retreat == true)
            {
                Vector2 direction = (target.gameObject.transform.position - transform.position).normalized;
                rb.velocity = -direction * speed;
            }
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
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            target = collision.gameObject;
            retreat = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            retreat = false;
        }
    }
    void triggerAnim(bool state)
    {

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
