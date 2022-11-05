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

    

    // Use this for initialization
    void Start()
    {
        
        Virus = GameObject.Find("Virus");
        //timer = 4;
        PlaqisChasing = false;
        AttractionSpeed = 6;

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

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            PlaqisChasing = false;
        }
    }


    void Update()
    {
        /*
        if (BossBehaviour01.EnemyCurrentState == 3)
        {
            PlaqisChasing = true;
        }

        //TO CHASE VIRUS
        if (PlaqisChasing == true)
        {
            AttractionSpeed = 6f;
            transform.position = Vector3.MoveTowards(transform.position, Virus.transform.position, AttractionSpeed * Time.deltaTime);
        }*/
        if (PlaqisChasing == true)
        { 
            transform.position = Vector3.MoveTowards(transform.position, Virus.transform.position, AttractionSpeed * Time.deltaTime);

        }
        else if (PlaqisChasing == false) { 
        
        }



    }

    
}
