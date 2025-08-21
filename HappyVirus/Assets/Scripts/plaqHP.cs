using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaqHP : MonoBehaviour
{
    public GameObject mainPlaq;
    public GameObject explosion;
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
            Instantiate(explosion, this.transform.position, transform.rotation);
            destroyThis();
        }
       
    }
    
    public void destroyThis()
    {
        Destroy(this.transform.parent.transform.parent.gameObject);
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
