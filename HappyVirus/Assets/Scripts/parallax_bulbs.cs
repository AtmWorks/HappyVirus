using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax_bulbs : MonoBehaviour
{
    public Transform cameraTransform;
    public float relativeMove = .3f;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("No se encontró una cámara en la escena.");
        }
    }
    void Update()
    {
        transform.position = new Vector2(cameraTransform.position.x * relativeMove, cameraTransform.position.y * relativeMove);
    }
}
