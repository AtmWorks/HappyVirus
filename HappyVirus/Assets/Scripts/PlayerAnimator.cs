using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    public Animator FaceAnimator;
    public Animator MiniFaceAnimator;
    public static bool IsInfecting;
    public static bool IsShooting;
    public static bool IsCreatingEgg;
    public static bool GetDmg;


	void Start ()
    {
        GetDmg = false;


    }
	
	
	void Update ()
    {

        if (Input.GetKey ("space") || GetDmg == true)
        {
            FaceAnimator.SetBool("CreatingEgg", true);
        }
        else
        {
            FaceAnimator.SetBool("CreatingEgg", false);
        }

        if (Input.GetMouseButton(0) && PlayerStatics.O2counter >=1)
        {
            
            FaceAnimator.SetBool("IsShooting", true);
            MiniFaceAnimator.SetBool("IsShooting", true);
            IsShooting = true;
        }
        else
        {
            FaceAnimator.SetBool("IsShooting", false);
            MiniFaceAnimator.SetBool("IsShooting", false);
            IsShooting = false;
        }


        if (GetDmg == true || IsShooting==true)
        {
            MiniFaceAnimator.SetBool("IsShooting", true);    
        }
        else
        { 
            MiniFaceAnimator.SetBool("IsShooting", false); 
        }
    
        if (IsInfecting == true)
        {
            FaceAnimator.SetBool("IsInfecting", true);
        }
        else
        {
            FaceAnimator.SetBool("IsInfecting", false);
        }
    }
    
}
