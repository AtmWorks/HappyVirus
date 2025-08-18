using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getSlimed : MonoBehaviour
{
    public Animator animator;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Virus" || collision.gameObject.tag == "Proyectil" || collision.gameObject.tag == "Damage" || collision.gameObject.tag == "OrangeChild")
        {
            animator.SetBool("isGettingHit", true);
            timer = 0;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.tag == "Proyectil" )
        {
            animator.SetBool("isGettingHit", true);
            timer = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            animator.SetBool("isGettingHit", false);

        }
    }
}
