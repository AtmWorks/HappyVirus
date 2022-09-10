using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTumorBehaviour : MonoBehaviour
{


    private Transform moveTo;
    public float ChaseSpeed;
    public bool isChasing;

    //rotacion
    public float RotSpeed;
    public int WichDirection = 0;

    void Start()
    {
        //EnemyAlive = true;
        isChasing = false;
        //target = GameObject.FindGameObjectsWithTag("Player").GetComponent<Transform>();

        WichDirection = Random.Range(0, 4);

        if (WichDirection >= 2)
        { RotSpeed = -5; }

        if (WichDirection <= 2)
        { RotSpeed = 5; }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus" && isChasing == false)
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
    void FixedUpdate()
    {
        transform.Rotate(0, 0, RotSpeed * Time.deltaTime);

        if (isChasing == true)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, moveTo.transform.position, ChaseSpeed * Time.deltaTime);
            ChaseSpeed = ChaseSpeed * 1.002f ;
            RotSpeed = RotSpeed * 1.005f;

            //Enemy01Face.enemy01FaceState = 2;
        }
        else
        {
            //Enemy01Face.enemy01FaceState = 1;
        }



    }
}