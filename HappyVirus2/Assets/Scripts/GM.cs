using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

    public Camera mainCamera;
    public float zoomSpeed =10f;
    public float maxZoom;
    public float minZoom;

    public void RestartLVL()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {

        //RESETEAR NIVEL
        if  (Input.GetKeyDown("n")) { RestartLVL(); }
        //AÑADIR OXIGENO GRATIS
        if (Input.GetKeyDown("o")) { PlayerStatics.O2counter += 10; }

        //SCROLL ZOOM DE LA CAMARA
            float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            mainCamera.orthographicSize -= scroll * zoomSpeed;
        }
        //LIMITES DEl ZOOM
        if(mainCamera.orthographicSize > maxZoom)
        {
            mainCamera.orthographicSize = maxZoom;
        }
        if (mainCamera.orthographicSize < minZoom)
        {
            mainCamera.orthographicSize = minZoom;
        }

    }
}
