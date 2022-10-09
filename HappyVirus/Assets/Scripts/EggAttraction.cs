using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAttraction : MonoBehaviour
{
   

    public float AttractionSpeed;
    public static bool isAbsorbing;
    public static bool isAtracting;
    public bool isInside;
    public float dedidedAtraction;

    private void Start()
    {
        isInside = false;
        dedidedAtraction = AttractionSpeed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Follow" && isAbsorbing == true && isInside ==false)
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, AttractionSpeed * 1.5f * Time.deltaTime);
    
        }
        if (collision.tag == "Follow" && isAbsorbing == true && isInside ==true)
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, (AttractionSpeed/5) * Time.deltaTime);
    
        }
        if (collision.tag == "EggAtractor" && isAtracting == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, AttractionSpeed* 1.5f * Time.deltaTime);

        }

    }
}
