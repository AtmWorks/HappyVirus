using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpBehaviour : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            parent.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
