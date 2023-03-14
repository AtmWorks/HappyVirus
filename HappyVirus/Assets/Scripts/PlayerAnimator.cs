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
	
	
	void FixedUpdate ()
    {
        if (IsShooting)
        {
            
            FaceAnimator.SetBool("IsShooting", true);//esto se ejecuta
            MiniFaceAnimator.SetBool("IsShooting", true);//esto no se ejecuta

        }
        else
        {
            FaceAnimator.SetBool("IsShooting", false);
            MiniFaceAnimator.SetBool("IsShooting", false);
        }
        if ( GetDmg==true || IsCreatingEgg == true)
        {
            FaceAnimator.SetBool("CreatingEgg", true);//esto se ejecuta

            MiniFaceAnimator.SetBool("IsShooting", true); //esto se ejecuta   
        }
        else
        {
            FaceAnimator.SetBool("CreatingEgg", false);//esto se ejecuta
            //MiniFaceAnimator.SetBool("IsShooting", false); //esto se ejecuta
        }
    
        if (IsInfecting == true)
        {
            FaceAnimator.SetBool("IsInfecting", true);//esto se ejecuta
        }
        else
        {
            FaceAnimator.SetBool("IsInfecting", false);//esto se ejecuta
        }
    }
    
}
