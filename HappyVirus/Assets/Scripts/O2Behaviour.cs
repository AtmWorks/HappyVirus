using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2Behaviour : MonoBehaviour {

    
    //public PlayerStatics Static;
    private bool getOnce;
    public bool canChase;
    private bool isChasing;
    private GameObject target;
    public Collider2D thisCollider;
    public float chaseSpeed = 3.0f;
    void Start () {
        getOnce = true;
        target = null;
        this.transform.rotation = new Quaternion(0, 0, 0, 0); ;
	}
    void Update ()
    {
        if (isChasing)
            {
            // Calcula la dirección hacia el objetivo
            Vector3 direction = (target.transform.position - transform.position).normalized;
            // Mueve el objeto suavemente hacia el objetivo
            transform.position += direction * chaseSpeed * Time.deltaTime;
            chaseSpeed += Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Virus" && getOnce == true && canChase == true)
        {
            target = other.gameObject;
            isChasing = true;
            thisCollider.enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Virus" && getOnce == true)
        {
            StartCoroutine(destroyDelay());
        }
    }

    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(0.02f);
        getOnce = false;
        PlayerStatics.O2counter += 2;
        popScore.popText= "+2";
        popScore.isPoping = true;
        Destroy(gameObject);
    }
}
