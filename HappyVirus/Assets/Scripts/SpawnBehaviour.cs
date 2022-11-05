using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour
{
    public Animator bubble01;
    public Animator bubble02;
    public Animator bubble03;
    public bool isInfected;

    void Start()
    {
        isInfected = false;   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Virus")
        {
            
            bubble01.SetBool("Infected", true);
            bubble02.SetBool("Infected", true);
            bubble03.SetBool("Infected", true);
            isInfected = true;
        }
    }
    
    void Update()
    {
        

    }
}
