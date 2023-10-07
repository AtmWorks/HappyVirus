using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaqHP : MonoBehaviour
{
    public GameObject mainPlaq;
    public bool isDestroy;
    // Start is called before the first frame update
    void Start()
    {
        isDestroy = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Proyectil")
        {
            destroyThis();
        }
       
    }
    
    public void destroyThis()
    {
        Destroy(transform.parent.gameObject);

    }
    // Update is called once per frame
    void Update()
    {
        if (isDestroy)
        {
            destroyThis ();
        }
    }
}
