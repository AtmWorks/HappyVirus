using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialText : MonoBehaviour
{
    public GameObject message;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            message.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Virus") 
        {
            message.SetActive(false);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
