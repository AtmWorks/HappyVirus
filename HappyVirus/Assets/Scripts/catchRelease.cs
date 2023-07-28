using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catchRelease : MonoBehaviour
{

    [SerializeField] private Tentacle tentacle1;
    public Rigidbody2D spawnTarget;
    public bool isOnTentacleTarget;
    public static bool releaseAll;
    public GameObject tentacleTarget;
    private bool LostTentacleRange = false;
    float radius;
    bool noColliders = false;
    float checkInterval = 2f;


    public void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "tentacleTarget")
        {
            //Tentaculo encuentra un objetivo y lo guarda
            isOnTentacleTarget = true;
            tentacleTarget = collision.gameObject;
        }

    }
    public void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "tentacleRange")
        {
            //si salimos del rango devolvemos el target
            tentacle1.Release();
            tentacle1.TargetRigidbody = spawnTarget;
            isOnTentacleTarget = false;
        }
        if (collision.tag == "tentacleTarget" && tentacle1.IsHoldingTarget==false)
        {

            tentacle1.TargetRigidbody = spawnTarget;
            isOnTentacleTarget = false;
        }
    }

    void Start()
    {
        releaseAll = false;
        isOnTentacleTarget = false;

        radius = GetComponent<CircleCollider2D>().radius;
        StartCoroutine(CheckForColliders());
    }

    IEnumerator CheckForColliders()
    {
        while (true)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            noColliders = true;
            foreach (Collider2D col in colliders)
            {
                if (col.tag == "tentacleRange")
                {
                    noColliders = false;
                    break;
                }
            }
            if (noColliders)
            {
                // Aquí si no se encuentran colliders con el tag "tentacleRange"

                tentacle1.TargetRigidbody = spawnTarget;
                isOnTentacleTarget = false;

            }
            yield return new WaitForSeconds(checkInterval);
        }
    }
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1) && isOnTentacleTarget == true)
        {

            releaseAll = true;
            if (tentacleTarget != null)
            {
                if (tentacle1.IsHoldingTarget)
                {
                    tentacle1.Release();
                }
                else
                {
                    releaseAll = false;
                    Rigidbody2D targetRigidbody = tentacleTarget.GetComponent<Rigidbody2D>();
                    tentacle1.TargetRigidbody = targetRigidbody;
                    tentacle1.Catch();
                }

            }
            else
            {
                tentacle1.TargetRigidbody = spawnTarget;
                isOnTentacleTarget = false;
                releaseAll = true;
            }
        }

        if (isOnTentacleTarget == true)
        {
            Rigidbody2D targetRigidbody = tentacleTarget.GetComponent<Rigidbody2D>();
            tentacle1.TargetRigidbody = targetRigidbody;


        }
        else
        {
            tentacle1.TargetRigidbody = spawnTarget;
            tentacle1.Release();

        }
        if (releaseAll==true)
        {
            tentacle1.Release();
        }

    }
}
