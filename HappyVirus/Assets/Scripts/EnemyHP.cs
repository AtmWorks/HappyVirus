using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

    public GameObject explosion;
    public GameObject realExplosion;
    public GameObject EnemyObject;
    public bool EnemyAlive;
    public int enemyHP;
	// Use this for initialization
	void Start ()
    {
        //enemyHP = 2;
	}

    void EnemyDies()
    {
        Destroy(EnemyObject);
        //EnemyObject.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.tag == "Proyectil")
        { Debug.Log("ENEMY HIT"); enemyHP--; Debug.Log(enemyHP.ToString()); }
        if (collisionTrig.gameObject.tag == "Virus")
        {
            Instantiate(realExplosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);

            EnemyDies();
        }


    }
    // Update is called once per frame
    void Update () {

   
        if (enemyHP <= 0)
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            
            EnemyDies();
        }

        

        
    }
}
