using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

    public GameObject explosion;
    public GameObject realExplosion;
    public GameObject EnemyObject;
    public GameObject face;
    public SpriteRenderer bodyRend;
    public GameObject corpse;
    public Animator thisAnim;
    public ChasePlayer chaseScript;
    public float deadlyTimer;

    public bool isInfected;
    public List<SimpleFlash> flashList;
    public Material flashMaterial;

    public ChasePlayer parentTag;

    //[SerializeField] private SimpleFlash flashEffectbody;
    //[SerializeField] private SimpleFlash flashEffectface;

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
        if (isInfected)
        {
            StartCoroutine(infectDelay());
        }
    }

    void EnemyDies()
    {
        //bodyRend.material = flashMaterial;
        this.gameObject.tag = "invisible";
        if (parentTag != null )
        {
            parentTag.isDead = true;
        }
        EnemyAlive = false;
        if (chaseScript != null) { 
        chaseScript.enabled = false;
        }
        if (face != null) { 
            face.SetActive(false);
        }
        if(thisAnim != null)
        {
            thisAnim.SetBool("isAttack", false);
            thisAnim.SetBool("isDead", true);
        }


    }

    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.tag == "Proyectil")
        {
            foreach (SimpleFlash flash in flashList)
            {
                flash.Flash();
            }
            //Debug.Log("ENEMY HIT"); 
            //flashEffectface.Flash();
            //flashEffectbody.Flash();
            enemyHP--; 
            //Debug.Log(enemyHP.ToString()); 
        }
    }

    public IEnumerator infectDelay()
    {

        thisAnim.SetBool("isInfected", true);
        yield return null;

    }
    void FixedUpdate () {


        //if (enemyHP <= 0 && EnemyAlive == true)
        //{
        //    Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        //    Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        //    Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        //    EnemyDies();
        //}
        //if (EnemyAlive == false)
        //{
        //    deadlyTimer -= Time.deltaTime;
        //}

        //if(deadlyTimer <= 0.5f && isSpawned)
        //{
        //    Instantiate(corpse, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        //    isSpawned = false;
        //}
        //if(deadlyTimer <= 0f)
        //{
        //    Destroy(EnemyObject);
        //}
        if (enemyHP <= 0 )
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Instantiate(corpse, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(EnemyObject);

        }

    }
}
