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

        

        if ( IsShooting==true)
        {
            FaceAnimator.SetBool("IsShooting", true);
            MiniFaceAnimator.SetBool("IsShooting", true);    
        }
        else
        {
            FaceAnimator.SetBool("IsShooting", false);
            MiniFaceAnimator.SetBool("IsShooting", false);
        }
        if ( GetDmg==true || IsCreatingEgg == true)
        {
            FaceAnimator.SetBool("CreatingEgg", true);
            
            MiniFaceAnimator.SetBool("IsShooting", true);    
        }
        else
        {
            FaceAnimator.SetBool("CreatingEgg", false);
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
