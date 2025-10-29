using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proyectilBone : MonoBehaviour{

    public GameObject explosion;
    public bool proyectHit;
    public float currentTime;
    public float speed;
    public float fireRate;
    //a rigidbody2d component
    private Rigidbody2D rb;

    void Start()
    {
        proyectHit = false;
        currentTime = 3f;
        rb = GetComponent<Rigidbody2D>();
    }
    IEnumerator destroyAfter()
    {
        yield return new WaitForSeconds(0.15f);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        Destroy(this.gameObject);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if ( collision.gameObject.tag == "Wall")
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
        if ( collision.gameObject.tag == "Virus")
        {
            StartCoroutine(destroyAfter());
        }


    }  
    private void OnColliderEnter2D(Collider2D collision)
    {

        if ( collision.gameObject.tag == "Wall" )
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
        if ( collision.gameObject.tag == "Virus")
        {
            StartCoroutine(destroyAfter());
        }


    }
    void Update()
    {

        currentTime -= 1 * Time.deltaTime;
        //Option1: apply speed to  transform 
        // if (speed != 0)
        // {
        //     transform.position += transform.right * (speed * Time.deltaTime);
        // }
        // else
        // {
        //     Debug.Log("No Speed");
        // }
        //option2: apply speed to rigidbody, also point to the direction of this rotation
        if (speed != 0)
        {
            rb.linearVelocity = transform.right * speed;
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
        // 
    }
}
