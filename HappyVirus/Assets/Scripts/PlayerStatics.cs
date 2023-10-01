using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatics : MonoBehaviour {

    public static int creationState;
    public static int O2counter;
    public static int maxO2counter;
    public static int EggsCounterPubl;
    public static int VirusState;
    public static float inmuneTimer;//
	// Use this for initialization
	void Start ()
    {
        creationState = 0;
        O2counter = 0;
        VirusState = 1;
        maxO2counter = 20;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (PlayerStatics.inmuneTimer > 0)
        {
            PlayerStatics.inmuneTimer -= 1 * Time.deltaTime;
        }
        if(O2counter > maxO2counter)
        {
            O2counter = maxO2counter;
        }
    }
}
