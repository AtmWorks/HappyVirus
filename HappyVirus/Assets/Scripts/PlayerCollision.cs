using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    
    public GameObject explosion;
    public GameObject spawn;
    public static int PlayermaxHP;
    public static int PlayerHP ;
    public float InmuneTime ;
    public float FaceTime ;
   // public GameObject PlayerObject;

	void Start ()
    {
        PlayermaxHP = 3;
        PlayerHP = 3;
        
    }

    void PlayerDies ()
    {
        //this.gameObject.SetActive(false);
        this.transform.position = spawn.transform.position;
        PlayerHP = 3;
    }

    private void OnCollisionStay2D(Collision2D PlayerCol )
    {
        if (PlayerCol.gameObject.tag == "Damage" && InmuneTime <= 0 && FaceTime <= 0)
        {
            // I only want one contact, so that's why I initialise it with capacity 1
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            PlayerCol.GetContacts(contacts);
            // you can get the point with this:
            var contactPoint = contacts[0].point;
            Debug.Log(contactPoint);
            // Do something with that point

            PlayerAnimator.GetDmg = true;

            Instantiate(explosion, new Vector3(contactPoint.x, contactPoint.y, 0), Quaternion.identity);

            PlayerHP -= 1;
            Debug.Log("ENEMY TOUCH");
            
            InmuneTime = 1f;
            FaceTime = 1f;

        }
        



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spawn")
        {
            spawn = collision.gameObject;
            Debug.Log("IM ON SPAWN");

        }
    }


    void Update ()
    {
        if (InmuneTime > 0)
        { 
		InmuneTime -= 1*Time.deltaTime;
        }

        if (FaceTime > 0)
        { 
		FaceTime -= 1*Time.deltaTime;
        }

        if (FaceTime <= 0)
        {
            PlayerAnimator.GetDmg = false;
        }

        if(PlayerHP <= 0)
        { PlayerDies();
            Debug.Log("I DIED");
        }
       
    }
}
