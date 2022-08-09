using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaqHP : MonoBehaviour
{
    public GameObject mainPlaq;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Proyectil")
        {
            Debug.Log("plaquetaHURTING"); 
            Destroy(transform.parent.gameObject);

        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
