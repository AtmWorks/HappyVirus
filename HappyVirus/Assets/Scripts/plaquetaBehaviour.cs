using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaquetaBehaviour : MonoBehaviour
{
    public GameObject Virus;
    public GameObject Boss;
    public float AttractionSpeed;
    public bool PlaqisChasing;
    public float timer;
    public Animator animator;
    

    // Use this for initialization
    void Start()
    {
        
        Virus = GameObject.Find("Virus");
        //timer = 4;
        PlaqisChasing = false;
        AttractionSpeed = 6;
        animator = GetComponent<Animator>();

    }

   


    //TO CHASE BOSS
    private void OnTriggerStay2D(Collider2D collision)
    {
        /* if (collision.gameObject.tag == "PlaquetaSpawner" && PlaqisChasing == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, AttractionSpeed * Time.deltaTime);
        } */
        if (collision.gameObject.tag == "Virus" )
        {
            PlaqisChasing = true;
        }

    }




    void Update()
    {

        if (PlaqisChasing == true)
        {
            AttractionSpeed += (Time.deltaTime/2);
            transform.position = Vector3.MoveTowards(transform.position, Virus.transform.position, AttractionSpeed * Time.deltaTime);

        }
        else if (PlaqisChasing == false) 
        { 
        
        }
        float distance = Vector3.Distance(Virus.transform.position, this.transform.position);
        if(distance < 2 ) 
        {
            animator.SetBool("isExplode", true);

        }


    }

    
}
