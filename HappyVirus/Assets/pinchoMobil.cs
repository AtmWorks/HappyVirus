using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinchoMobil : MonoBehaviour
{

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            anim.SetBool("triggered", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            anim.SetBool("triggered", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
