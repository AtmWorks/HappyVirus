using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Face : MonoBehaviour {

    private SpriteRenderer rend;
    public SpriteRenderer bodyrend;
    public Sprite normalFace, followFace, deadFace, normalBody, deadBody;
    public GameObject thisEnemy;
    private ChasePlayer chasePlayer;
    public EnemyHP enemyHp;

    void Start ()
    {
        chasePlayer = thisEnemy.GetComponent<ChasePlayer>(); ;
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = normalFace;

	}
	
	// Update is called once per frame
	void Update () {
        //if (chasePlayer.isChasing == true && enemyHp.EnemyAlive == true)
        //{ rend.sprite = followFace; }
        //else { rend.sprite = normalFace; }
        if (enemyHp.EnemyAlive == false)
        { 
            //rend.sprite = deadFace; 
            //bodyrend.sprite = deadBody;
            chasePlayer.enabled = false;
            this.gameObject.tag = "tentacleTarget";
            //TO DO: DESACTIVA EL CIRCLE COLLIDER 2D DE ESTE OBJETO
            GetComponent<CircleCollider2D>().enabled = false;

        }
        else
        {
            //bodyrend.sprite = normalBody;
            chasePlayer.enabled = true;
        }
		
	}
}
