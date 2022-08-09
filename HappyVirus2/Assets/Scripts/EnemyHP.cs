using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

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
       EnemyObject.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.tag == "Proyectil")
        { Debug.Log("ENEMY HIT"); enemyHP--; Debug.Log(enemyHP.ToString()); }
        
        
    }
    // Update is called once per frame
    void Update () {

   
        if (enemyHP <= 0)
        {
            EnemyDies();
        }

        

        
    }
}
