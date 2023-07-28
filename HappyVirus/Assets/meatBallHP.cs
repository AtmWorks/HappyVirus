using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meatBallHP : MonoBehaviour
{
    public GameObject explosion;
    //public GameObject EnemyObject;
    public Animator animator;
    public bool EnemyAlive;
    public int enemyHP;
    // Use this for initialization
    void Start()
    {
        EnemyAlive = true;
        enemyHP = 6;
    }

    void EnemyDies()
    {
        EnemyAlive = false;
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

        if (enemyHP <= 3) { animator.SetBool("isInfected", true); }

        if (enemyHP <= 0 && EnemyAlive == true)
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);

            EnemyDies();
        }

    }
}