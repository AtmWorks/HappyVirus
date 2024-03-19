using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZoomTrigger : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomSpeed = 0.5f;
    public float targetZoom;
    public float zoomEpsilon = 0.01f; // Un pequeño valor para determinar cuándo la cámara ha llegado al zoom deseado.

    private bool isZooming = false;
    private void Start()
    {
        Camera mainCameraAtScene = Camera.main;
        if (mainCameraAtScene != null)
        {
            mainCamera = mainCameraAtScene;
        }
        else
        {
            Debug.LogError("No se encontró una cámara en la escena.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Virus" && !isZooming)
        {
            // Inicia el zoom suave hacia targetZoom
            StartCoroutine(ZoomToTarget());
        }
    }

    IEnumerator ZoomToTarget()
    {
        isZooming = true;
        float initialZoom = mainCamera.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            // Interpola suavemente entre el valor actual y el objetivo
            mainCamera.orthographicSize = Mathf.Lerp(initialZoom, targetZoom, elapsedTime);

            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime * zoomSpeed;

            yield return null;
        }

        // Asegúrate de que la cámara alcance exactamente el valor objetivo
        mainCamera.orthographicSize = targetZoom;

        // Comprueba si la cámara ha llegado al zoom deseado
        while (Mathf.Abs(mainCamera.orthographicSize - targetZoom) > zoomEpsilon)
        {
            mainCamera.orthographicSize = targetZoom;
            yield return null;
        }

        isZooming = false;
    }
}
