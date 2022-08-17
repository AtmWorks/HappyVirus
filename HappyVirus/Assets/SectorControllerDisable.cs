using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorControllerDisable : MonoBehaviour
{
    public GameObject toDisable;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            if (toDisable.activeSelf == true) 
            { 
                toDisable.SetActive(false);

            }

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
