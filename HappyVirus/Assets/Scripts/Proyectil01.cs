using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil01 : MonoBehaviour {

    public GameObject explosion;
    public bool proyectHit;
    public float currentTime;
    public float speed;
    public float fireRate;
   // public GameObject shockCollider;
    void Start()
    {
        //shockCollider.SetActive(false);
        proyectHit = false;
        currentTime = 5f;
        StartCoroutine(enableHitCollider());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag=="Enemy" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "hit" )
        {
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
