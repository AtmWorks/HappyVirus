using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{

   // public GameObject wall;
    public Animator wallAnim;

    void Start()
    {
        wallAnim.SetBool("Open", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EGG")
        {
            wallAnim.SetBool("Open", true);
        }
    }
}
