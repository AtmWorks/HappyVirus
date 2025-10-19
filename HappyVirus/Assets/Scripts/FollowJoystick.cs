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

    [Header("Control por Mouse")]
    public bool isMouseControll = true;

    [Tooltip("Si está activo, el cursor se limitará al radio incluso con mouse o joystick.")]
    public bool isCursorLimited = false;

    // Update is called once per frame
    void Update()
    {
        // --- CONTROL POR MOUSE ---
        if (isMouseControll)
        {
            cursor.SetActive(true);
            limitCursor.SetActive(isCursorLimited); // Solo visible si está activado

            // Posición del mouse en el mundo
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            // Si el cursor está limitado, aplicar el radio
            if (isCursorLimited)
            {
                Vector3 direction = mouseWorldPos - Player2.transform.position;
                float distance = direction.magnitude;

                if (distance > radius)
                {
                    direction = direction.normalized * radius;
                    mouseWorldPos = Player2.transform.position + direction;
                }
            }

            // Actualizar posición
            transform.position = mouseWorldPos;
            return; // Evitar la lógica del joystick
        }

        // --- CONTROL POR JOYSTICK ---
        Vector2 joystickInput = new Vector2(rightJoystick.Horizontal, rightJoystick.Vertical);

        if (joystickInput != Vector2.zero)
        {
            cursor.SetActive(true);
            limitCursor.SetActive(isCursorLimited); // Solo visible si está activado

            Vector3 targetPosition = Player2.transform.position + new Vector3(joystickInput.x, joystickInput.y, 0) * radius;

            // Limitar la distancia al radio máximo
            if (isCursorLimited)
            {
                float distanceToTarget = Vector3.Distance(Player2.transform.position, targetPosition);
                if (distanceToTarget > radius)
                {
                    targetPosition = Player2.transform.position + (targetPosition - Player2.transform.position).normalized * radius;
                }
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
