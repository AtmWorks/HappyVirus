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

        // Obtén la dirección del joystick derecho en un vector
        Vector2 joystickInput = new Vector2(rightJoystick.Horizontal, rightJoystick.Vertical);

        // Asegúrate de que la dirección no sea igual a (0,0)
        if (joystickInput != Vector2.zero)
        {
            cursor.SetActive(true);
            // Calcula la posición objetivo
            Vector3 targetPosition = Player2.transform.position + new Vector3(joystickInput.x, joystickInput.y, 0) * radius;

            // Limita la distancia desde Player2 al radio máximo
            float distanceToTarget = Vector3.Distance(Player2.transform.position, targetPosition);
            if (distanceToTarget > radius)
            {
                targetPosition = Player2.transform.position + (targetPosition - Player2.transform.position).normalized * radius;
            }

            // Establece la posición del objeto actual al objetivo
            transform.position = targetPosition;
        }
        else { cursor.SetActive(false); }
    }
}
