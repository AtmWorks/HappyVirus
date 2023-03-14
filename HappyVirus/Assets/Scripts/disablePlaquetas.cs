using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class disablePlaquetas : MonoBehaviour
{
    public GameObject explosion;
    public GameObject O2;
    public GameObject parent;
    private float timer = 0.0f;
    public bool isInfected;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        isInfected = false;
      
        //isInfected = false;
    }

    void explode ()
    {
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        Instantiate(O2, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);

        parent.gameObject.SetActive(false);
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Virus") 
        {
            anim.SetBool("infection", true);

        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            timer += Time.deltaTime;
        }
    }    
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            timer -= Time.deltaTime;
            anim.SetBool("infection", false);

        }
    }
    // Update is called once per frame
    private void Update()
    {
        //if ()
        if (timer < 0) { timer = 0; }
        if (timer > 2) { isInfected= true; }
        if (isInfected) explode();
    }
}
