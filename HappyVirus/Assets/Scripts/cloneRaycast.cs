using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneRaycast : MonoBehaviour
{
    // Start is called before the first frame update
    public float visionDistance = 40;
    public float rotationSpeed = 30;
    public GameObject parent;
    public GameObject rayTarget;
    public ParticleFireBase thisFirePoint;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        parent.transform.Rotate(Vector3.forward*rotationSpeed * Time.deltaTime);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, rayTarget.transform.position, visionDistance);
        
        if(hitInfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            Debug.Log("Raycast hit "+hitInfo.collider.gameObject.tag);


        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + rayTarget.transform.position * visionDistance, Color.blue);
        }
    }
}
