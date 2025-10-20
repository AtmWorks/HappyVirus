using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowJoystick : MonoBehaviour
{
    public GameObject Player2;
    public float radius;
    public Joystick rightJoystick;
    public GameObject cursor;
    public GameObject limitCursor;

    // Update is called once per frame
    void Update()
    {
        // --- CONTROL POR MOUSE ---
        if (PlayerStatics.controlType == PlayerStatics.ControlScheme.PC)
        {
            mouseController();
        }
        else if (PlayerStatics.controlType == PlayerStatics.ControlScheme.Mobile)
        {
            joyStickController();
        }

        // --- CONTROL POR JOYSTICK ---
        
    }

    public void mouseController()
    {
        cursor.SetActive(true);

        // Posición del mouse en el mundo
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos;
    }

    public void joyStickController()
    {
        Vector2 joystickInput = new Vector2(rightJoystick.Horizontal, rightJoystick.Vertical);

        if (joystickInput != Vector2.zero)
        {
            cursor.SetActive(true);
            limitCursor.SetActive(true);
            Vector3 targetPosition = Player2.transform.position + new Vector3(joystickInput.x, joystickInput.y, 0) * radius;
            // Limitar la distancia al radio máximo
            float distanceToTarget = Vector3.Distance(Player2.transform.position, targetPosition);
            if (distanceToTarget > radius)
            {
                targetPosition = Player2.transform.position + (targetPosition - Player2.transform.position).normalized * radius;
            }
            transform.position = targetPosition;
        }
        else
        {
            cursor.SetActive(false);
            limitCursor.SetActive(false);
        }
    }
}
