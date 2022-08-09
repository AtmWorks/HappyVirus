using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusRot : MonoBehaviour {
    public float RotSpeed; 
    // Use this for initialization
    void Start () {
        RotSpeed = 6;

}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, RotSpeed * Time.deltaTime);
    }
}
