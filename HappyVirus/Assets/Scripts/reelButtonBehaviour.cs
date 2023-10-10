using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class reelButtonBehaviour : MonoBehaviour
{
    public UnityEngine.UI.Button boton01;
    public List<GameObject> buttons;
    public bool isOpen;
    public Image thisRend;
    public GameObject joy1;
    public GameObject joy2;
    public Camera mainCamera;
    float initialZoom;
    public float time;
    public float time2;
    // Start is called before the first frame update
    void Start()
    {
        initialZoom = mainCamera.orthographicSize;

        isOpen = false;
        boton01.onClick.AddListener(openReel);
        thisRend = this.gameObject.GetComponent<Image>();
        // thisRend.enabled = false;
        Color currentColor = thisRend.color;
        currentColor.a = 0f;
        thisRend.color = currentColor;
    }


    public void openReel()
    {
        if(isOpen)
        {
            foreach (GameObject go in buttons)
            {
                go.SetActive(true);
            }
            joy1.SetActive(false);
            joy2.SetActive(false);
            //  thisRend.enabled = true;
            Color currentColor = thisRend.color;
            currentColor.a = 1f;
            thisRend.color = currentColor;
            initialZoom = mainCamera.orthographicSize;
            //mainCamera.orthographicSize = 7;
            //Time.timeScale = 0.5f;

        }
        if (!isOpen)

        {
            foreach (GameObject go in buttons)
            {
                go.SetActive(false);
            }
            joy1.SetActive(true);
            joy2.SetActive(true);
            Color currentColor = thisRend.color;
            currentColor.a = 0f;
            thisRend.color = currentColor;
            //thisRend.Color = new Color con la opacidad al 0;
           // mainCamera.orthographicSize = initialZoom;
            //Time.timeScale = 1f;


        }
        isOpen = !isOpen;
    }
    // Update is called once per frame

}
