using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorControllerEnable : MonoBehaviour
{
    public GameObject toEnable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            if (toEnable.activeSelf == false)
            {
                toEnable.SetActive(true);

            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
