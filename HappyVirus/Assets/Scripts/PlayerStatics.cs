using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatics : MonoBehaviour {


    public enum ControlScheme { Mobile, PC, Controller }
    [Header("Esquema de control")]
    [Tooltip("Mobile = joystick en pantalla; PC = WASD/Flechas; Controller = (pendiente)")]
    public ControlScheme controlScheme = ControlScheme.Mobile;
    public static ControlScheme controlType;
    // public static int creationState;
    public static int O2counter;
    public static int redCurrCounter;
    public static int blueCurrCounter;
    public static int yellowCurrCounter;
    public static int maxO2counter;
    public static int EggsCounterPubl;
    public static float inmuneTimer;//
    public static int colorState = 0;

	// Use this for initialization
	void Start ()
    {
        controlType = controlScheme;
        // creationState = 0;
        O2counter = 0;
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
        if (controlType != controlScheme)
        {
            controlType = controlScheme;
        }
    }
}
