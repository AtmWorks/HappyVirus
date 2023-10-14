using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil01 : MonoBehaviour {

    //public GameObject audioCarry;
    //public audioAndDestroy audioAndDestroy;
    public GameObject explosion;
    public bool proyectHit;
    public float currentTime;
    public float speed;
    public float fireRate;
    public bool isYellowProyectile;
    public yellowProyectileTrigger trigger;
    public float rotationSpeed;

   // public GameObject shockCollider;
    void Start()
    {
        instanceSound();
        proyectHit = false;
        currentTime = 5f;
        StartCoroutine(enableHitCollider());
    }

    void instanceSound()
    {
        // Instanciar el objeto audioCarry
        //GameObject audioInstance = Instantiate(audioCarry, transform.position, Quaternion.identity);

        // Obtener el componente audioAndDestroy del objeto audioCarry instanciado
        //audioAndDestroy = audioInstance.GetComponent<audioAndDestroy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag=="Enemy" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "hit" )
        {
            audioAndDestroy.proyectilSound = true;
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }

        
    }

    IEnumerator enableHitCollider()
    {
        yield return new WaitForSeconds(0.1f);
       // shockCollider.SetActive(true);
    }

    void Update()
    {
        //audioCarry.transform.position=this.transform.position;  
        currentTime -= 1 * Time.deltaTime;
        if (speed != 0)
        {
            transform.position += transform.right * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No Speed");
        }
        if (currentTime <= 0)
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
        if(isYellowProyectile)
        {
            rotateTowardsEnemy();
        }
    }

    public void rotateTowardsEnemy()
    {
        if(trigger.enemy != null)
        {
            // Calcula la dirección hacia el enemigo
            Vector3 direction = trigger.enemy.transform.position - transform.position;

            // Calcula el ángulo en radianes
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Interpola la rotación suavemente (puedes ajustar el valor del tercer parámetro para cambiar la velocidad de rotación)
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
