using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    [SerializeField] private GameObject item;
    private bool onRange;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            item.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            item.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            item.SetActive(false);
        }
    }



}
