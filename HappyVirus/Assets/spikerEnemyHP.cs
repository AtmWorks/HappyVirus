using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikerEnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp;


    void Start()
    {
        hp = 6;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Proyectil") 
        {
            
        }
    }
}
