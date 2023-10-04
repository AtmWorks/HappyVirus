using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disablerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject disabler;
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            if (disabler.activeSelf == false) { disabler.SetActive(true); }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            if (disabler.activeSelf == true) { disabler.SetActive(false); }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
