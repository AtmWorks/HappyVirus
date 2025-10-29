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
    public static int PlayerHP;
    public static int PlayermaxHP;

    public CameraEffectController cameraEffectController;

    // Use this for initialization
    void Start ()
    {
        cameraEffectController = FindFirstObjectByType<CameraEffectController>();
        controlType = controlScheme;
        // creationState = 0;
        O2counter = 0;
        maxO2counter = 20;
        PlayermaxHP = 1;
        PlayerHP = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerStatics.inmuneTimer > 0)
        {
            PlayerStatics.inmuneTimer -= 1 * Time.deltaTime;
        }
        if (O2counter > maxO2counter)
        {
            O2counter = maxO2counter;
        }
        if (controlType != controlScheme)
        {
            controlType = controlScheme;
        }
    }
    
    public void substractHP(int dmg)
    {
        PlayerHP -= dmg;
        // cameraEffectController.triggerFade();
        cameraEffectController.triggerShake(0.4f,0.4f);
    }
}
