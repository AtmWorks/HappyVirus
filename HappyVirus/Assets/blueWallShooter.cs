using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueWallShooter : MonoBehaviour
{
    public GameObject virus = null;
    public GameObject proyectil;
    public Transform shootPoint;
    public float rotationSpeed;
    public float initialRotation;
    public Animator thisAnimator;
    public float timeBetweenAttacks;
    public bool attack = false;
    public float timer;
    public GameObject objetToRotate;

    void Start()
    {
        initialRotation = objetToRotate.transform.rotation.eulerAngles.z;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("+++ENTERING SHOOTING AREA");
        if (other.gameObject.tag == "Virus")
        {
            if (virus == null)
            virus = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        virus = null;
    }

    private void Update()
    {
        // Actualizar el temporizador restando el tiempo transcurrido desde el último frame
        timer -= Time.deltaTime;

        if (virus != null)
        {
            //TODO: rotate objetToRotate towards virus 
            Vector3 direction = virus.transform.position - objetToRotate.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            objetToRotate.transform.rotation = Quaternion.Slerp(objetToRotate.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            float angleDifference = Quaternion.Angle(objetToRotate.transform.rotation, targetRotation);
            if (angleDifference < 1f && timer <= 0)
            {
                thisAnimator.SetBool("canAttack", true);

            }
        }
        else
        {
            //TODO: rotate towards initial rotation
            Quaternion targetRotation = Quaternion.Euler(0, 0, initialRotation);
            objetToRotate.transform.rotation = Quaternion.Slerp(objetToRotate.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (attack && timer < 0 )
        {
            attack = false;
            Instantiate(proyectil, shootPoint.position, shootPoint.rotation);
            timer = timeBetweenAttacks;
            attack = false;
            thisAnimator.SetBool("canAttack", false);
        }
    }
}