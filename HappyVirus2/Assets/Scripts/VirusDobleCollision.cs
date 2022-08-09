using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusDobleCollision : MonoBehaviour {

    public int VirusHP;
   
    // public GameObject PlayerObject;

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
            VirusHP++;
            VirusDies();
            Debug.Log("MINI VIRUS DIED");
        }

    }
}
