using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gutsBehaviour : MonoBehaviour
{
    public float comebackSpeed;
    public bool inSide;
    public GameObject sourcePoint;
    public float returnTime = 1.0f; // Tiempo que tarda en volver a su posición original

    private float comebackTimer = 0.0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            inSide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            inSide = false;
            comebackTimer = returnTime; // Reinicia el temporizador cuando sale del rango del virus
        }
    }

    void FixedUpdate()
    {
        if (inSide == false)
        {
            comebackTimer -= Time.deltaTime;
            float lerpValue = Mathf.Clamp01(1 - (comebackTimer / returnTime)); // Calcula el valor de interpolación

            transform.position = Vector3.Lerp(transform.position, sourcePoint.transform.position, lerpValue);
        }
    }
}
