using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popScore : MonoBehaviour
{
    public GameObject popObject;
    public GameObject popObjectCenter;
    public float startTImer;
    public static string popText;
    public static string popTextCenter;
    public static bool isPoping;
    public static bool isPopingCenter;
    private Vector3 startPosition;
    private Color textColor;
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
        if (popObjectCenter != null)
        {
            if (isPopingCenter)
            {
                startTImer = 0;
                popObjectCenter.SetActive(true);
                popObjectCenter.GetComponent<Text>().text = popTextCenter;
                popObjectCenter.GetComponent<Text>().color = textColor;
                transform.position = startPosition;
                isPopingCenter = false;
            }
            if (!isPopingCenter && startTImer > 1f)
            {
                popObjectCenter.SetActive(false);
            }
            if (popObjectCenter.activeSelf)
            {
                Vector3 newPosition = transform.position;
                newPosition.y += 100*Time.deltaTime;
                transform.position = newPosition;
                popObjectCenter.GetComponent<Text>().color = Color.Lerp(popObjectCenter.GetComponent<Text>().color, new Color(textColor.r, textColor.g, textColor.b, 0f), Time.deltaTime/2);
            }
        }
    }
}

