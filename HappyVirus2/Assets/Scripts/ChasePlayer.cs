using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour {

   
    private Transform moveTo;
    public float ChaseSpeed;
    public bool isChasing;

    void Start()
    {
        //EnemyAlive = true;
        isChasing = false;
        //target = GameObject.FindGameObjectsWithTag("Player").GetComponent<Transform>();
    }

 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Virus" && isChasing == false )
        {
            isChasing = true;
           
            moveTo = collision.transform;

            

        }


    }
    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.tag == "Virus" && isChasing == true)
        {
            isChasing = false;

          


        }
    }
    // Update is called once per frame
    void FixedUpdate ()
    {
        
        if (isChasing==true)
        {
            this.transform.position = Vector3.MoveTowards(transform.position,moveTo.transform.position, ChaseSpeed * Time.deltaTime);
            //Enemy01Face.enemy01FaceState = 2;
        }
        else
        {
            //Enemy01Face.enemy01FaceState = 1;
        }
        


    }
}
