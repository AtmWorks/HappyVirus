using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    public GameObject virusParent;
    public GameObject explosion;
    public GameObject spawn;
    public HPbar hpscript;
    public static int PlayermaxHP;
    public static int PlayerHP ;
    public static float InmuneTime ;
    public static float FaceTime ;
    public static bool gotDamage;
   // public GameObject PlayerObject;

	void Start ()
    {
        PlayermaxHP = 3;
        PlayerHP = 3;
        gotDamage = false;
    }

    void PlayerDies ()
    {
        //this.gameObject.SetActive(false);
        virusParent.transform.position = spawn.transform.position;
        //this.transform.parent.transform.position = spawn.transform.position;
        PlayerHP = 3;
    }

    void oneDamage()
    {
        PlayerHP -= 1;
    }
    private void OnCollisionStay2D(Collision2D PlayerCol )
    {
        if (PlayerCol.gameObject.tag == "Damage" && PlayerStatics.inmuneTimer <= 0 && FaceTime <= 0)
        {
            
            // I only want one contact, so that's why I initialise it with capacity 1
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            PlayerCol.GetContacts(contacts);
            // you can get the point with this:
            var contactPoint = contacts[0].point;
            //Debug.Log(contactPoint);
            // Do something with that point


            Instantiate(explosion, new Vector3(contactPoint.x, contactPoint.y, 0), Quaternion.identity);

            Debug.Log("ENEMY TOUCH");

            gotDamage = true;
            


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


    void FixedUpdate ()
    {

        FaceTime -= 1 * Time.deltaTime;
        if (FaceTime <= 0)
        {
            PlayerAnimator.GetDmg = false;
        }

        if(PlayerHP <= 0)
        { PlayerDies();
            Debug.Log("I DIED");
        }

        if (gotDamage == true && PlayerStatics.inmuneTimer <= 0)
        {
            PlayerAnimator.GetDmg = true;
            PlayerStatics.inmuneTimer = 1f;
            FaceTime = 1f;
            oneDamage();
            //hpscript.updateHP();
            gotDamage = false;



        }

    }
}
