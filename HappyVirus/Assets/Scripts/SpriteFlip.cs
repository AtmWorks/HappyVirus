using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour {

    public Joystick screenJoystick;
    public Joystick screenJoystick2;

    public bool inPC;
    public bool inMobile;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 characterScale = transform.localScale;
        if (inPC)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                characterScale.x = -1;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                characterScale.x = 1;
            }
        }

        if (inMobile)
        {
            float joystickHorizontalInput = screenJoystick.Horizontal;
            float joystickHorizontalInput2 = screenJoystick2.Horizontal;

            if (joystickHorizontalInput < 0 || joystickHorizontalInput2 < 0)
            {
                characterScale.x = -1;
            }
            else if (joystickHorizontalInput > 0 || joystickHorizontalInput2 > 0)
            {
                characterScale.x = 1;
            }
            transform.localScale = characterScale;
        }

    }
}
