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
    public RandomMovement thisRandScript;
    public bool canExplode;

    // Use this for initialization
    void Start()
    {
        canExplode = false;
        this.gameObject.tag = "Neutral";
        Virus = GameObject.Find("Virus");
        //timer = 4;
        PlaqisChasing = false;
        AttractionSpeed = 6;
        animator = GetComponent<Animator>();
        thisRandScript = GetComponent<RandomMovement>();
    }

   


    //TO CHASE BOSS
    private void OnTriggerEnter2D(Collider2D collision)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Virus")
        {
            canExplode = true;
        }
    }


    void Update()
    {

        if (PlaqisChasing == true)
        {
            thisRandScript.enabled = false;
            AttractionSpeed += (Time.deltaTime/2);
            transform.position = Vector3.MoveTowards(transform.position, Virus.transform.position, AttractionSpeed * Time.deltaTime);

        }
        else if (PlaqisChasing == false) 
        { 
        
        }
        float distance = Vector3.Distance(Virus.transform.position, this.transform.position);
        if(distance < 2 || canExplode ) 
        {
            animator.SetBool("isExplode", true);
            StartCoroutine(changeTag());
        }


    }

    IEnumerator changeTag()
    {
        yield return new WaitForSeconds(0.8f);
        //PlaqisChasing=false;
        this.gameObject.tag = "Damage";

    }
}
