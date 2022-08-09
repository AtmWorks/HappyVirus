using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorBehaviour : MonoBehaviour
{
    public GameObject popDialogue;
    public Animator popManager;
    //public string popUpText;

    // Start is called before the first frame update
    void Start()
    {
        popDialogue.SetActive(false);

       //PopUpSystem pop = popManager.GetComponent<PopUpSystem>();
        
    }

    public void closeDialog ()
    {
        popManager.SetBool("Open", false);
        popManager.SetBool("Close", true);
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus" && Input.GetKey("e"))
        {
            Debug.Log("DIALOGUE GOING ON");
            popDialogue.SetActive(true);
            popManager.SetBool("Open", true);
            popManager.SetBool("Close", false);
            
            //PopUpSystem pep = popManager.GetComponent<PopUpSystem>();
            //PopUpSystem pep = GameObject.FindGameObjectsWithTag("GameManager").GetComponent<PopUpSystem>();
            //pep.PopUpText(popUp);
        }
    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus" )
        {
            //popDialogue.SetActive(false);
            closeDialog();
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
