using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusDobleCollision : MonoBehaviour {

    public int VirusHP;
   
    public GameObject explosion;

    void Start()
    {
        VirusHP = 1;
        

    }

    void VirusDies()
    {
        
        this.gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D PlayerCol)
    {
        if (PlayerCol.gameObject.tag == "Damage" )
        {
            VirusHP -= 1;
            Debug.Log("ENEMY TOUCH MINI VIRUS");

           

        }


    }


    void Update()
    {
       

        if (VirusHP <= 0)
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            VirusHP++;
            VirusDies();
            Debug.Log("MINI VIRUS DIED");
        }

    }
}
