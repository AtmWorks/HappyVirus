using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2Behaviour : MonoBehaviour
{


    //public PlayerStatics Static;
    private bool didSound;
    private bool getOnce;
    public bool canChase;
    private bool isChasing;
    private GameObject target;
    public Collider2D thisCollider;
    public float chaseSpeed = 3.0f;
    public string tagToFollow = string.Empty;
    public enum currType
    {
        red,
        yellow,
        blue,
        o2
    };

    public currType objType;
    void Start()
    {
        didSound = false;
        getOnce = true;
        target = null;
        this.transform.rotation = new Quaternion(0, 0, 0, 0); ;
    }
    void Update()
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
        if (tagToFollow != null)
        {
            if (other.gameObject.tag == "Virus" && getOnce == true && canChase == true)
            {
                thisCollider.enabled = true;
                target = other.gameObject;
                isChasing = true;
            }
        }
        else
        {
            if (other.gameObject.tag == tagToFollow && getOnce == true && canChase == true)
            {
                thisCollider.enabled = true;
                target = other.gameObject;
                isChasing = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (tagToFollow != string.Empty)
        {
            if (other.gameObject.tag == tagToFollow && getOnce == true)
            {
                //PlayerCollision audioTime = other.gameObject.GetComponent<PlayerCollision>();
                //if (!didSound)
                //{
                //    didSound = true;
                //    //////audioTime.reproduceO2Sound();
                //}
                StartCoroutine(destroyDelay());
            }
        }
        else
        {
            if (other.gameObject.tag == "Virus" && getOnce == true)
            {
                //PlayerCollision audioTime = other.gameObject.GetComponent<PlayerCollision>();
                //if (!didSound)
                //{
                //    didSound = true;
                //    //////audioTime.reproduceO2Sound();
                //}
                StartCoroutine(destroyDelay());
            }
        }

    }

    IEnumerator destroyDelay()
    {
        getOnce = false;
        yield return new WaitForSeconds(0.1f);
        switch (objType)
        {
            case currType.o2:
                PlayerStatics.O2counter += 1;
                popScore.popText = "+1";
                popScore.isPoping = true;
                Destroy(this.gameObject);
                break;
            case currType.red:
                PlayerStatics.redCurrCounter += 1;
                //popScore.popText = "+1";
                //popScore.isPoping = true;
                Destroy(this.gameObject);
                break;
            case currType.blue:
                PlayerStatics.blueCurrCounter += 1;
                //popScore.popText = "+1";
                //popScore.isPoping = true;
                Destroy(this.gameObject);
                break;


        }
        Destroy(this.gameObject);


    }
}