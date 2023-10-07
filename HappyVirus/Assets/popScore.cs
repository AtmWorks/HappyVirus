using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popScore : MonoBehaviour
{
    public GameObject popObject;
    public float startTImer;
    public static string popText;
    public static bool isPoping;
    private Vector3 startPosition;
    private Color textColor;
    private float opacity;
    private void Start()
    {
        startPosition = transform.position;
        textColor = popObject.GetComponent<Text>().color;

        popText = "";
        popObject.GetComponent<Text>().text = popText;
    }

    private void Update()
    {
        startTImer += Time.deltaTime;
        if (popObject != null)
        {
            if (isPoping)
            {
                startTImer = 0;
                popObject.SetActive(true);
                popObject.GetComponent<Text>().text = popText;
                popObject.GetComponent<Text>().color = textColor;
                transform.position = startPosition;
                isPoping = false;
            }
            if (!isPoping && startTImer > 1f)
            {
                popObject.SetActive(false);
            }
            if (popObject.activeSelf)
            {
                Vector3 newPosition = transform.position;
                newPosition.x += 50*Time.deltaTime;
                transform.position = newPosition;
                popObject.GetComponent<Text>().color = Color.Lerp(popObject.GetComponent<Text>().color, new Color(textColor.r, textColor.g, textColor.b, 0f), 2f * Time.deltaTime);
            }
        }
    }
}

