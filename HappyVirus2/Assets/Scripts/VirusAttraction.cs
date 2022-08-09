using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusAttraction : MonoBehaviour {

    public GameObject VirusNexus;
    public float AttractionSpeed;
    public bool isChasing;
    // Use this for initialization
    void Start () {
        isChasing = true;
        AttractionSpeed = 4;

    }
	
	// Update is called once per frame
	void Update () {
        if (isChasing == true)
        {
            AttractionSpeed = 5f;
            transform.position = Vector3.MoveTowards(transform.position, VirusNexus.transform.position, AttractionSpeed * Time.deltaTime);
        }
        if (isChasing==false)
        {
            AttractionSpeed = 0;
            transform.position = this.transform.position;
        }
        
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "VirusNexus")
        { isChasing = false; } ;
        Debug.Log("Im INSIDE");
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "VirusNexus")
        { isChasing = true; };
        Debug.Log("Im OUTSIDE");

    }
}
