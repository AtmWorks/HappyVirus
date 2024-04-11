using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (proceduralBehaviour.difficutly == 0)
        {
            if (baseLvl1.Count > 0)
            {
                // Baraja la lista baseLvl1
                List<GameObject> shuffledList = baseLvl1.OrderBy(x => Random.value).ToList();

                // Activa el primer objeto de la lista barajada
                shuffledList[0].SetActive(true);
            }
            else
            {
                Debug.LogWarning("No possible objects to activate.");
            }
        }
        else if (proceduralBehaviour.difficutly == 1)
        {
            if (baseLvl2.Count > 0)
            {
                // Baraja la lista baseLvl1
                List<GameObject> shuffledList = baseLvl2.OrderBy(x => Random.value).ToList();

                // Activa el primer objeto de la lista barajada
                shuffledList[0].SetActive(true);
            }
            else
            {
                Debug.LogWarning("No possible objects to activate.");
            }
        }


    }

}
