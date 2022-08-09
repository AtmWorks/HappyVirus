using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatics : MonoBehaviour {

    public static int creationState;
    public static int O2counter;
    public static int EggsCounterPubl;
    public static int VirusState; //
	// Use this for initialization
	void Start ()
    {
        creationState = 0;
        O2counter = 0;
        VirusState = 1;

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
