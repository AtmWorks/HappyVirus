using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour {

    public Joystick screenJoystick;
    public Joystick screenJoystick2;

    public GameObject fakeJoystick1;
    public GameObject fakeJoystick2;

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
            
            float joystickVerticallInput = screenJoystick.Vertical;
            float joystickVerticalInput2 = screenJoystick2.Vertical;

            Vector2 leftFakeJoy = new Vector2(joystickHorizontalInput, joystickVerticallInput);
            Vector2 rightFakeJoy = new Vector2(joystickHorizontalInput2, joystickVerticalInput2);

            if (joystickHorizontalInput2 < 0)
            {

                characterScale.x = -1;
            }
            if (joystickHorizontalInput < 0 && joystickHorizontalInput2 == 0)
            {

                characterScale.x = -1;
            }
            if (joystickHorizontalInput2 > 0)
            {
                characterScale.x = 1;
            }
            if (joystickHorizontalInput > 0 && joystickHorizontalInput2 == 0)
            {
                characterScale.x = 1;
            }

            if (leftFakeJoy != Vector2.zero)
            {
                fakeJoystick1.SetActive(false);
            }
            else { fakeJoystick1.SetActive(true);}
            
            if (rightFakeJoy != Vector2.zero)
            {
                fakeJoystick2.SetActive(false);
            }
            else { fakeJoystick2.SetActive(true);}

            transform.localScale = characterScale;
        }

    }
}
