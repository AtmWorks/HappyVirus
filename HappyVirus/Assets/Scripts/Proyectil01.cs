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
    public bool canChase;
    public bool isYellowProyectile;
    public float rotationSpeed;
    public GameObject enemyTag;

   // public GameObject shockCollider;
    void Start()
    {
        canChase = false;
        instanceSound();
        proyectHit = false;
        currentTime = 5f;
        StartCoroutine(StartRotation());
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

        if (collision.gameObject.tag=="Enemy" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "hit" || collision.gameObject.tag == "neutral" )
        {
            //audioAndDestroy.proyectilSound = true;
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
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            float minDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy;
                }
            }
            enemyTag = nearestEnemy;
        }

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
        if(isYellowProyectile && canChase)
        {
            rotateTowardsEnemy();
        }
    }

    public void rotateTowardsEnemy()
    {
        if(enemyTag != null)
        {
            // Calcula la dirección hacia el enemigo
            Vector3 direction = enemyTag.transform.position - transform.position;

            // Calcula el ángulo en radianes
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Interpola la rotación suavemente (puedes ajustar el valor del tercer parámetro para cambiar la velocidad de rotación)
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    IEnumerator StartRotation()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (isYellowProyectile)
            {
                canChase = true;
            }
            yield return null;
        }
    }
}
