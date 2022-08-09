using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobuloRojo : MonoBehaviour {

    public GameObject O2;
    public Animator GlobuloAnim;
    public float currentTime;
    public bool isTouched;
    public GameObject parent;
	// Use this for initialization
	void Start () {
        currentTime = 3.1f;
        isTouched = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Virus")
        {

            GlobuloAnim.SetBool("IsInfected", true);
            isTouched = true;
           // Instantiate(O2, transform.position, transform.rotation);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            PlayerAnimator.IsInfecting = true;
            Debug.Log("IM INFECTING");
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Virus")
        {
            PlayerAnimator.IsInfecting = false;
            Debug.Log("IM NOT INFECTING");
        }
    }
    // Update is called once per frame
    void Update ()
    {
        if (isTouched == true)
        {
            currentTime -= 1 * Time.deltaTime;
        }
        if (currentTime <= 0.5f)
        {
            Instantiate(O2, transform.position, transform.rotation);
            isTouched = false;
            currentTime = 6;
            

        }
        if (currentTime >= 6) { currentTime += 1 * Time.deltaTime; }
        if (currentTime >= 7f)
        {
           
            Destroy(parent);
        }
        
	}
}
