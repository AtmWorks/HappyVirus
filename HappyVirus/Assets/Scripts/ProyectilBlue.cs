using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilBlue : MonoBehaviour {

    public GameObject explosion;
    public bool proyectHit;
    public float currentTime;
    public float speed;
    public float fireRate;

    void Start()
    {
        proyectHit = false;
        currentTime = 5f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "hit")
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }

        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Virus"))
    //    {
    //        // Comprueba si el otro collider es un trigger
    //        if (!collision.collider.isTrigger)
    //        {
    //            // Si no es un trigger, realiza la acción
    //            Instantiate(explosion, transform.position, Quaternion.identity);
    //            Destroy(gameObject);
    //        }
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Virus"))
        {

            // Si no es un trigger, realiza la acción
            StartCoroutine(destroyAfter());
            
        }
    }

    IEnumerator destroyAfter()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void Update()
    {
        
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
    }
}
