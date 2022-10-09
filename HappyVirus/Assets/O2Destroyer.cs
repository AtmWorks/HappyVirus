using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2Destroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "O2")
        {
            //collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        
        }
    }
}
