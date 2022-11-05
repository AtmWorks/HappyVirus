using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject Player;
    public GameObject NextLocation;
    public bool didTP;
    void Start()
    {
        didTP = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player) {
            didTP = false;
            Debug.Log("I FOUND THE PLAYER");
            teleportNext();
        }
    }

    public void teleportNext()
    {
        if (didTP == false)
        {
            Player.transform.position = NextLocation.transform.position;
            didTP = true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
