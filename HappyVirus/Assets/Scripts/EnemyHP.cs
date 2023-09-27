using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

    public GameObject explosion;
    public GameObject realExplosion;
    public GameObject EnemyObject;
    public GameObject corpse;
    public Animator thisAnim;
    public ChasePlayer chaseScript;
    public float deadlyTimer;

    [SerializeField] private SimpleFlash flashEffectbody;
    [SerializeField] private SimpleFlash flashEffectface;

    public bool EnemyAlive;
    public bool isSpawned;
    public int enemyHP;

	// Use this for initialization
	void Start ()
    {
        EnemyAlive = true;
        isSpawned = true;

        deadlyTimer = 1.5f;
        //enemyHP = 2;
    }

    void EnemyDies()
    {
        EnemyAlive = false;
        thisAnim.SetBool("isDead", true);
        thisAnim.SetBool("isAttack", false);

        chaseScript.enabled = false;


    }

    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.tag == "Proyectil")
        {
            //Debug.Log("ENEMY HIT"); 
            flashEffectface.Flash();
            flashEffectbody.Flash();
            enemyHP--; 
            //Debug.Log(enemyHP.ToString()); 
        }
    }

    void Update () {

   
        if (enemyHP <= 0 && EnemyAlive == true)
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
           // Instantiate(corpse, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            
            EnemyDies();
        }
        if (EnemyAlive == false)
        {
            deadlyTimer -= Time.deltaTime;
        }

        if(deadlyTimer <= 0.5f && isSpawned)
        {
            Instantiate(corpse, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            isSpawned = false;
        }
        if(deadlyTimer <= 0f)
        {
            EnemyObject.SetActive(false);
        }

    }
}
