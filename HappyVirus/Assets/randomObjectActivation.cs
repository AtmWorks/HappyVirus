using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomObjectActivation : MonoBehaviour
{
    public List<GameObject> baseLvl1 = new List<GameObject>();
    public List<GameObject> baseLvl2 = new List<GameObject>();
    public List<GameObject> yellowLvl1 = new List<GameObject>();
    void Start()
    {
        foreach (GameObject obj in baseLvl1) 
        {
            obj.SetActive(false);
        }
        ActivateRandomObject();
    }

    void ActivateRandomObject()
    {
        if (baseLvl1.Count > 0)
        {
            int randomIndex = Random.Range(0, (baseLvl1.Count));
            baseLvl1[randomIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("No possible objects to activate.");
        }
        
    }

}
