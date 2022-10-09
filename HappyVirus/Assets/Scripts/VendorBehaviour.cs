using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorBehaviour : MonoBehaviour
{
    public GameObject popDialogue;
    public GameObject popDialogue2;
    public GameObject popDialogue3;
    public GameObject Espam;
    public bool didActive;
    public int counter;
    public bool pacing;
    public float timer;
    //public Animator popManager;
    //public string popUpText;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        didActive = false;
        popDialogue.SetActive(false);
        pacing = false;
        timer = 1f;
        
    }

 
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus" && didActive == false && counter == 0)
        {
            Espam.SetActive(true);
        }

        if (collision.gameObject.tag == "Virus" && Input.GetKey("e")&& pacing == false)
        {
            pacing = true;
            Debug.Log("I SHOWED FIRST");
            Espam.SetActive(false);
            didActive = true;
            if (timer <= 0) {
                switch (counter)
                {
                    case 0:
                        //desactiva
                        popDialogue.SetActive(true);
                        //counter = 1;
                        pacing = false;
                        Debug.Log("TEXT 1");
                        //timer = 1;
                        break;
                    case 1:
                        //desactiva
                        popDialogue.SetActive(false);
                        popDialogue2.SetActive(true);
                        counter = 2;
                        pacing = false;
                        Debug.Log("TEXT 2");
                        timer = 1;

                        break;
                    case 2:
                        //desactiva
                        popDialogue2.SetActive(false);
                        popDialogue3.SetActive(true);
                        counter = 2;
                        pacing = false;
                        Debug.Log("TEXT 3");
                        timer = 1;
                        break;
                }

            }
            

         
        }
        
     
    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus" )
        {
            Espam.SetActive(false);
            popDialogue.SetActive(false);
            popDialogue2.SetActive(false);
            popDialogue3.SetActive(false);

            didActive = false;
            counter = 0;
            pacing = false;

        }
    }

     void FixedUpdate()
    {
        Debug.Log("FIXED UPDATE");

        if (timer>= (-1f))
        {
            Debug.Log("Timer is up");
            timer -= Time.deltaTime;

        }
    }
    // Update is called once per frame

}
