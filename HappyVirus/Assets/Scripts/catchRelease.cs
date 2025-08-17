using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class catchRelease : MonoBehaviour
{

    [SerializeField] private Tentacle tentacle1;
    public Rigidbody2D spawnTarget;
    public GameObject indicator;
    public GameObject spawnTransform;

    public UnityEngine.UI.Button boton01;


    public bool isOnTentacleTarget;
    public static bool releaseAll;
    public GameObject tentacleTarget;
    //private bool LostTentacleRange = false;
    float radius;
    bool noColliders = false;
    float checkInterval = 2f;

    public Transform rangeAnchor;
    public float desiredDistance;
    public bool showGizmos = true;

    public void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "tentacleTarget")
        {
            //Tentaculo encuentra un objetivo y lo guarda
            isOnTentacleTarget = true;
            tentacleTarget = collision.gameObject;
            indicator.SetActive(true);
        }

    }
    public void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "tentacleRange")
        {
           
        }
        if (collision.tag == "tentacleTarget" && tentacle1.IsHoldingTarget==false)
        {
            tentacle1.TargetRigidbody = spawnTarget;
            isOnTentacleTarget = false;
            tentacleTarget = null;
        }
    }
    public void triggerOutOfRange()
    {
        //si salimos del rango devolvemos el target
        if (tentacle1.IsHoldingTarget && tentacleTarget != null)
        {
            tentacle1.Release();
        }
        tentacle1.TargetRigidbody = spawnTarget;
        isOnTentacleTarget = false;
        tentacleTarget = null;
    }

    void Start()
    {
        releaseAll = false;
        isOnTentacleTarget = false;
        tentacleTarget=null;
        radius = GetComponent<CircleCollider2D>().radius;
        StartCoroutine(CheckForColliders());
        boton01.onClick.AddListener(catchAndRelease);
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

    public void catchAndRelease()
    {
        if (isOnTentacleTarget == true)
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
        
        
    }

    void FixedUpdate()
    {
        //si el actual transform se aleja 'desiredDistance' (float) de rangeAnchor (transform declarado en el inspector), ejecuta triggerOutOfRange()
        if (rangeAnchor != null)
        {
            float dist = Vector2.Distance(transform.position, rangeAnchor.position);
            if (dist > desiredDistance)
            {
                triggerOutOfRange();
            }
        }

        if (tentacleTarget==null && tentacle1.IsHoldingTarget == false)
        {
            indicator.SetActive(false);
        }
        else { indicator.SetActive(true); }
        if (Input.GetMouseButtonDown(1) && isOnTentacleTarget == true)
        {
            catchAndRelease();
        }

        if (isOnTentacleTarget == true)
        {
            if(tentacleTarget!=null)
            {
                Rigidbody2D targetRigidbody = tentacleTarget.GetComponent<Rigidbody2D>();
                tentacle1.TargetRigidbody = targetRigidbody;

            }
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

    private void OnDrawGizmos()
    {
        if (showGizmos && rangeAnchor != null)
        {
            Gizmos.color = Color.magenta; // rosa

            int segments = 64; // más segmentos = círculo más suave
            float angleStep = 2 * Mathf.PI / segments;
            Vector3 prevPoint = rangeAnchor.position + new Vector3(Mathf.Cos(0), Mathf.Sin(0), 0) * desiredDistance;

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep;
                Vector3 nextPoint = rangeAnchor.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * desiredDistance;
                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }
        }
    }
}
