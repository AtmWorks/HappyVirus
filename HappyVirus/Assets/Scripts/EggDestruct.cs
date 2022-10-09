using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDestruct : MonoBehaviour {

    //Transform parent;
    public GameObject mainEgg;
    public EggAttraction mainAttraction;
	// Use this for initialization
	void Start () {
        
        //parent = transform.parent;
	}

    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "EggAtractor" && PlayerStatics.creationState == 1)
        {
           Destroy(mainEgg);
        }
        if(other.tag == "Follow")
        {
           mainAttraction.isInside = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Follow")
        {
            mainAttraction.isInside = false;
        }
    }
    void Update ()
    {
		
	}
}
