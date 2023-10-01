using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
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
    [SerializeField] private List<SimpleFlash> flashList;

    // Use this for initialization
    void Start()
    {
        Alive = true;
        enemyHP = 9;
    }

    void EnemyDies()
    {
        

        //Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);


       Alive = false;
    }
    IEnumerator flashDMG()
    {

        foreach (SimpleFlash flash in flashList)
        {
            flash.Flash();

        }
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.tag == "Proyectil")
        {

            enemyHP--; 
            StartCoroutine(flashDMG());
        }
    }

    void Update()
    {

        if (enemyHP <= 7 && enemyHP> 5) { 
            bodyrend.sprite = infectedBody;
        }
        if (enemyHP <= 3 && enemyHP > 0)  
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