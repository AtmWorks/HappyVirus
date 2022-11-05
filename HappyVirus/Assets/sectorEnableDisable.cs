using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sectorEnableDisable : MonoBehaviour
{

    public GameObject objToEnable;
    public GameObject objToDisable;
    public bool isEnabler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus"&& isEnabler ==true)
        {
            objToEnable.SetActive(true);
            Debug.Log("SI HAS PASADOW");

        }
        if (collision.tag == "Virus"&& isEnabler ==false)
        {
            
            objToDisable.SetActive(false);
            Debug.Log("SI HAS PASADO");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
