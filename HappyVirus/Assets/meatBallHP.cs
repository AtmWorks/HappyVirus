using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meatBallHP : MonoBehaviour
{
    public GameObject explosion;
    //Sprites de enfermo

    public SpriteRenderer bodyrend;
    public Sprite normalBody, infectedBody, infectedPlusBody,deadBody;

    //public GameObject EnemyObject;
    public Animator animator;
    public bool Alive;
    public int enemyHP;
    public meatBall meatBall;
    // Use this for initialization
    void Start()
    {
        Alive = true;
        enemyHP = 8;
    }

    void EnemyDies()
    {
        

        //Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);


       Alive = false;
    }

    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.tag == "Proyectil")
        {
            Debug.Log("ENEMY HIT"); enemyHP--; Debug.Log(enemyHP.ToString());
        }
    }

    void Update()
    {

        if (enemyHP <= 6 && enemyHP> 4) { 
            bodyrend.sprite = infectedBody;
            Debug.Log("I CHANGED SPRITE");
        }
        if (enemyHP <= 4 && enemyHP > 0)  
        {
            bodyrend.sprite = infectedPlusBody;
            animator.SetBool("isInfected", true);

        }

        if (enemyHP <= 0 && Alive == true)
        {
            bodyrend.sprite = deadBody;

            animator.SetBool("isDead", true);
            meatBall.enabled = false;
            EnemyDies();
        }

    }
}