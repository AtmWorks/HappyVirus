using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAttraction : MonoBehaviour
{
   

    public float AttractionSpeed;
    public static bool isAbsorbing;
    public float maxDistance;


    private void Start()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Follow" && isAbsorbing == true)
        {
            Vector3 targetPosition = collision.transform.position;
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget > maxDistance)
            {
                // Si está a más de 25 unidades de distancia, mueve hacia la posición de destino a velocidad completa.
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, AttractionSpeed * Time.deltaTime);
            }
            else
            {
                // Si está a 25 unidades de distancia o menos, reduce la velocidad a cero utilizando Lerp.
                float lerpSpeed = Mathf.Lerp(AttractionSpeed, -4f, (maxDistance - distanceToTarget) / maxDistance);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
            }
        }


    }
}
