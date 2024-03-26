using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomObjectActivation : MonoBehaviour
{
    public List<GameObject> possibleObjects = new List<GameObject>();
    void Start()
    {
        foreach (GameObject obj in possibleObjects) 
        {
            obj.SetActive(false);
        }
        ActivateRandomObject();
    }

    void ActivateRandomObject()
    {
        if (possibleObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, (possibleObjects.Count-1));
            possibleObjects[randomIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("No possible objects to activate.");
        }
        
    }

}
