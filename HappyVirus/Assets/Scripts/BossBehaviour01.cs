using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour01 : MonoBehaviour
{

    //MOVEMENT
    private Transform moveTo;
    public float ChaseSpeed;
    public bool isChasing;

    //ATTACK
    public GameObject plaqSpawner1;
    public GameObject plaqSpawner2;
    public GameObject plaqSpawner3;
    public GameObject plaqSpawner4;
    public GameObject plaqSpawner5;
    public GameObject plaqSpawner6;
    public GameObject plaqSpawner7;
    public GameObject plaqSpawner8;

    public GameObject plaqueta;
    public float currentTime;
    public bool spawning;
    public float maxTime;
    public static int EnemyCurrentState;
    public Animator animBoss;
    public int bossHP;
    

    void Start()
    {

        bossHP = 1;
        spawning = false;
        isChasing = false;
        currentTime = 5f;
        maxTime = 5f;
        EnemyCurrentState = 1;
    }

    //CHASE PLAYER
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Virus" && isChasing == false && spawning == false)
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

   

    void spawn01 ()
    {

        //Spawn Plaquetas
        Instantiate(plaqueta, new Vector3(plaqSpawner1.transform.position.x, plaqSpawner1.transform.position.y, 0), Quaternion.identity);
        Instantiate(plaqueta, new Vector3(plaqSpawner2.transform.position.x, plaqSpawner2.transform.position.y, 0), Quaternion.identity);
        Instantiate(plaqueta, new Vector3(plaqSpawner3.transform.position.x, plaqSpawner3.transform.position.y, 0), Quaternion.identity);
        Instantiate(plaqueta, new Vector3(plaqSpawner4.transform.position.x, plaqSpawner4.transform.position.y, 0), Quaternion.identity);
        //Instantiate(plaqueta, new Vector3(plaqSpawner5.transform.position.x, plaqSpawner5.transform.position.y, 0), Quaternion.identity);
        //Instantiate(plaqueta, new Vector3(plaqSpawner6.transform.position.x, plaqSpawner6.transform.position.y, 0), Quaternion.identity);
        //Instantiate(plaqueta, new Vector3(plaqSpawner7.transform.position.x, plaqSpawner7.transform.position.y, 0), Quaternion.identity);
        //Instantiate(plaqueta, new Vector3(plaqSpawner8.transform.position.x, plaqSpawner8.transform.position.y, 0), Quaternion.identity);


    }
    
    private void Update()
    {

       
        //start timer if player in range
        if (isChasing == true)
        {
            currentTime -= 1 * Time.deltaTime;
        }

        //DELAY ENTRE SPAWNS
        if (currentTime >= maxTime)
        {
            currentTime = maxTime;
        }

        
       
        //ANIMACION
        if (currentTime <= 2.5f && isChasing == true && EnemyCurrentState == 1)
        {
            animBoss.SetBool("Shooting", true);
        }
        else
        {
            animBoss.SetBool("Shooting", false);
        }

        //SPAWN PLAQUETAS
        if (currentTime <= 0f && EnemyCurrentState==1 )
        {
            Debug.Log("BOSS SPAWNING PLAQUETAS");
            //spawnea plaquetas.
            spawn01();
            currentTime = maxTime;
            EnemyCurrentState = 2;
            //animBoss.SetBool("Shooting", false);


        }

        //CAST PLAQUETAS

        
        if (currentTime <= 0f && EnemyCurrentState == 2)
        {
            Debug.Log("BOSS MANDA PLAQUETAS");
         
           // plaquetaBehaviour.PlaqisChasing = true;
            currentTime = maxTime;
            EnemyCurrentState = 3;
            //animBoss.SetBool("Shooting", false);
        }
        
        if (currentTime <= 0f && EnemyCurrentState == 3)
        {
            Debug.Log("BOSS MANDA PLAQUETAS");

            // plaquetaBehaviour.PlaqisChasing = true;
            currentTime = maxTime;
            EnemyCurrentState = 1;
            //animBoss.SetBool("Shooting", false);
        }

    }

    void FixedUpdate()
    {

        //MOVE TOWARDS PLAYER
        if (isChasing == true )
        {
            this.transform.position = Vector3.MoveTowards(transform.position, moveTo.transform.position, ChaseSpeed * Time.deltaTime);
            
        }
    }
}
