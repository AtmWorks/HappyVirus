using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAttraction : MonoBehaviour
{
   

    public float AttractionSpeed;
    public static bool isAbsorbing;
    public static bool isAtracting;
    public bool isInside;
    public float maxDistance;

    private void Start()
    {
        isInside = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        //TODO: SUSTITUYE LO SIGUIENTE:
        //DESDE AQUI
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
        //HASTA AQUI
        //POR LO SIGUIENTE: transform.postion se movera (con lerp) hacia la posicion de collision mientras este mas lejos de 25 de distancia, pero cuando se encuentre a menos 25 de distancia, reduce (con lerp) la velocidad a zero. no es necesario utilizad el bool isInside
        if (collision.tag == "EggAtractor" && isAtracting == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, AttractionSpeed* 1.5f * Time.deltaTime);

        }

    }
}
