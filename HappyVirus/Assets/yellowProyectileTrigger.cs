using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yellowProyectileTrigger : MonoBehaviour
{
    public bool isOnTrigger;
    public GameObject enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Enemy"))
        {
            isOnTrigger = true;
            enemy = collision.gameObject;
        }
    }
}
