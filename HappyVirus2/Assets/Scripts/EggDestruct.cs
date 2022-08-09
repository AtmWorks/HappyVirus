using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDestruct : MonoBehaviour {

    //Transform parent;
    public GameObject mainEgg;
	// Use this for initialization
	void Start () {
        //parent = transform.parent;
	}

    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "EggAtractor" && PlayerStatics.creationState == 1)
        {
            Debug.Log("ITS WORKING");
           Destroy(mainEgg);
        }
    }

    void Update ()
    {
		
	}
}
