using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Face : MonoBehaviour {

    private SpriteRenderer rend;
    public Sprite normalFace, followFace;
    //public static int enemy01FaceState;
    public GameObject thisEnemy;
    private ChasePlayer chasePlayer;

    void Start ()
    {
        chasePlayer = thisEnemy.GetComponent<ChasePlayer>(); ;
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = normalFace;

	}
	
	// Update is called once per frame
	void Update () {
        if (chasePlayer.isChasing == true)
        { rend.sprite = followFace; }
        else { rend.sprite = normalFace; }
        
        //if (enemy01FaceState == 1) { rend.sprite = normalFace; }
        //if (enemy01FaceState == 2) { rend.sprite = followFace; }
		
	}
}
