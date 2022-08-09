using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
    public float RotSpeed;
    public int WichDirection = 0;
	// Use this for initialization
	void Start ()
    {
        WichDirection = Random.Range(0, 4);
        
        if (WichDirection >= 2)
        { RotSpeed = Random.Range(-50, -30); }

        if (WichDirection <= 2)
        { RotSpeed = Random.Range(50, 30); }
    }


    // Update is called once per frame
    void Update()
    {
       

        transform.Rotate(0, 0, RotSpeed * Time.deltaTime); //rotates 50 degrees per second around z axis
    }
}
