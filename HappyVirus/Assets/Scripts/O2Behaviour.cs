using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2Behaviour : MonoBehaviour {

    
    //public PlayerStatics Static;
    private bool getOnce;
    

    void Start () {
        getOnce = true;
       
        this.transform.rotation = new Quaternion(0, 0, 0, 0); ;
	}

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Virus" && getOnce == true)
        {
            getOnce = false;
            PlayerStatics.O2counter++;
            //Player.O2counter++;
            Debug.Log("+1 O2");
            Destroy(gameObject);
            
           // AmmoCounter.ammoValue += 1;


        }
    }
}
