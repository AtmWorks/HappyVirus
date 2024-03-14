using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleEnemyBehaviour : MonoBehaviour
{
    public bool virusDetected;
    public bool canChase;
    public Rigidbody2D thisRb;
    public GameObject virusObject;
    public float instantVelocity;
    public bool isChasing;
    public Animator thisAnimator;
    public bool firstAttack;
    public float desiredRotationForce;

    public void Start()
    {
        virusDetected = false;
        canChase = false;
        firstAttack = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!virusDetected)
        {
            if (collision.gameObject.tag == "Virus")
            {
                virusDetected = true;
                virusObject = GameObject.Find("Virus");
                thisAnimator.SetBool("canChase", true);
            }
        }
    }

    private void chaseSetup()
    {
        canChase = true;
        thisAnimator.SetBool("canChase", true);
    }

    private void Update()
    {

        if (virusDetected && virusObject != null && firstAttack == true)
        {
            Vector2 directionToVirus = ((Vector2)virusObject.transform.position - (Vector2)thisRb.position).normalized;
            float angleToVirus = Mathf.Atan2(directionToVirus.y, directionToVirus.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angleToVirus));
            thisRb.MoveRotation(Mathf.LerpAngle(thisRb.rotation, targetRotation.eulerAngles.z, Time.deltaTime * desiredRotationForce));
        }


        if (!canChase && !isChasing )
        {
            thisAnimator.SetBool("canChase", false);
            thisRb.velocity = Vector2.Lerp(thisRb.velocity, Vector2.zero, Time.deltaTime * 2f);
            if(virusObject != null) { 
            if (Vector2.Dot(thisRb.transform.right, (virusObject.transform.position - thisRb.transform.position).normalized) > 0.95f)
            {
                chaseSetup();
            }
            }
        }

        if (isChasing)
        {
            if(!firstAttack) { firstAttack = true; }
            thisRb.velocity = transform.right * instantVelocity;
        }

    }
}


