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

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
    void Update ()
    {
		
	}
}
