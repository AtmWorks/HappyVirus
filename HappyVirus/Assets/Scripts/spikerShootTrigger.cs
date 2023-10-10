using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class spikerShootTrigger : MonoBehaviour
{
    public spikerEnemyBehaviour behaviourScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            behaviourScript.canShoot = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            behaviourScript.canShoot = false;
        }
    }
}
