using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax_bulbs : MonoBehaviour
{
    public Transform cameraTransform;
    public float relativeMove = .3f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(cameraTransform.position.x * relativeMove, cameraTransform.position.y * relativeMove);
    }
}
