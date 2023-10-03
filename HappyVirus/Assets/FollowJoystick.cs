using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowJoystick : MonoBehaviour
{
    public GameObject Player2;
    public float radius;
    public Joystick rightJoystick;
    public GameObject cursor;

    // Update is called once per frame
    void Update()
    {

        // Obt�n la direcci�n del joystick derecho en un vector
        Vector2 joystickInput = new Vector2(rightJoystick.Horizontal, rightJoystick.Vertical);

        // Aseg�rate de que la direcci�n no sea igual a (0,0)
        if (joystickInput != Vector2.zero)
        {
            cursor.SetActive(true);
            // Calcula la posici�n objetivo
            Vector3 targetPosition = Player2.transform.position + new Vector3(joystickInput.x, joystickInput.y, 0) * radius;

            // Limita la distancia desde Player2 al radio m�ximo
            float distanceToTarget = Vector3.Distance(Player2.transform.position, targetPosition);
            if (distanceToTarget > radius)
            {
                targetPosition = Player2.transform.position + (targetPosition - Player2.transform.position).normalized * radius;
            }

            // Establece la posici�n del objeto actual al objetivo
            transform.position = targetPosition;
        }
        else { cursor.SetActive(false); }
    }
}
