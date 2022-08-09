using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCounter : MonoBehaviour {

    public static bool NexusFull;
    public GameObject DobleVirus;
    public GameObject DobleVirus1;
    public GameObject DobleVirus2;
    public GameObject DobleVirus3;
    public static int EggCount;
    public bool Icreated;
    public float currentTime;
    public static bool IsAbsorbing;

    public void Start()
    {
        EggCount = 0;
        IsAbsorbing = false;
        Icreated = false;
        currentTime = 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EGG")
        { 
        EggCount++;
        Debug.Log(string.Format("EggCount = {0}", EggCount));

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "EGG")
        {
            EggCount--;
            Debug.Log(string.Format("EggCount = {0}", EggCount));

        }
    }

    private void FixedUpdate()
    {
        if (DobleVirus.activeSelf == true && DobleVirus1.activeSelf == true && DobleVirus2.activeSelf == true && DobleVirus3.activeSelf == true) { NexusFull = true; }
        else if (DobleVirus.activeSelf == false || DobleVirus1.activeSelf == false || DobleVirus2.activeSelf == false || DobleVirus3.activeSelf == false) { NexusFull = false; }

        if (EggCount >=3 && Icreated == false && EggAttraction.isAtracting == true && NexusFull == false)
        {
            Icreated = true;
            PlayerStatics.creationState = 1;
            //Instantiate(DobleVirus, this.gameObject.transform.position, this.gameObject.transform.rotation);
            if(DobleVirus.activeSelf == false) { DobleVirus.SetActive(true); PlayerStatics.EggsCounterPubl -= 3; }

            else if (DobleVirus.activeSelf == true && DobleVirus1.activeSelf == false) { DobleVirus1.SetActive(true); PlayerStatics.EggsCounterPubl -= 3; }

            else if (DobleVirus.activeSelf == true && DobleVirus1.activeSelf == true && DobleVirus2.activeSelf == false) { DobleVirus2.SetActive(true); PlayerStatics.EggsCounterPubl -=3; }

            else if (DobleVirus.activeSelf == true && DobleVirus1.activeSelf == true && DobleVirus2.activeSelf == true && DobleVirus3.activeSelf == false) { DobleVirus3.SetActive(true); PlayerStatics.EggsCounterPubl -= 3; }


            
        }

        if (PlayerStatics.creationState == 1)
        {
            currentTime -= 1 * Time.deltaTime;
            EggAttraction.isAtracting = false;
        }

        if(currentTime <= 0f)
        {
            Icreated = false;
            currentTime = 0.1f;
            EggCount = 0;
            PlayerStatics.creationState = 0;
            this.gameObject.SetActive(false);
            
        }

    }


}
