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
    private float value ;

    void Start ()
    {
        GetDmg = false;
        value = 2f;

    }


    void FixedUpdate ()
    {
        value -= Time.deltaTime;
        if (IsShooting)
        {
            
            FaceAnimator.SetBool("IsShooting", true);
            MiniFaceAnimator.SetBool("IsShooting", true);

        }
        else
        {
            FaceAnimator.SetBool("IsShooting", false);
            MiniFaceAnimator.SetBool("IsShooting", false);
        }
        if ( IsCreatingEgg == true)
        {
            FaceAnimator.SetBool("CreatingEgg", true);
            MiniFaceAnimator.SetBool("IsShooting", true);    
        }
        else
        {
            FaceAnimator.SetBool("CreatingEgg", false);
            //MiniFaceAnimator.SetBool("IsShooting", false); 
        }

        if (GetDmg == true && value < 0)
        {
            FaceAnimator.SetBool("CreatingEgg", true);
            MiniFaceAnimator.SetBool("isDamage", true); 
            value = 1f;
        }
        else
        {
            //FaceAnimator.SetBool("CreatingEgg", false);
            MiniFaceAnimator.SetBool("isDamage", false);   
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
