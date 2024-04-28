using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class randomObjectActivation : MonoBehaviour
{
    public List<GameObject> baseLvl1 = new List<GameObject>();
    public List<GameObject> baseLvl2 = new List<GameObject>();
    public List<GameObject> yellowTransition = new List<GameObject>();
    public List<GameObject> yellowLvl1 = new List<GameObject>();
    public List<GameObject> spawnsList = new List<GameObject>();
    public List<int> transitionsInOrder = new List<int>();
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
        //Red basic
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
        //red complex
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
        else if (proceduralBehaviour.difficutly == 2)
        {
            if (yellowTransition.Count > 0)
            {
                // Baraja la lista baseLvl1
                List<GameObject> shuffledList = yellowTransition.OrderBy(x => Random.value).ToList();

                //TODO: 
                List<string> orientations = new List<string> { "L", "R", "T", "B" };
                //Foreach string inside the enum
                foreach (string orientation in orientations)
                {
                    if (shuffledList[0].name.Contains(orientation))
                    {
                        foreach (GameObject spawn in spawnsList)
                        {
                            //FALTA EXCLUIR LOS QUE TIENEN MAPA
                            if (spawn.name.Contains(orientation))
                            {
                                spawnProcedural spawnComponent = spawn.GetComponent<spawnProcedural>();
                                if (spawnComponent != null)
                                {
                                    if(spawnComponent.mapToSpawn == null)
                                    {
                                        spawnComponent.nextLvlType = 1;
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning($"spawnProcedural component not found on {spawn.name}.");
                                }
                            }
                        }
                    }
                }

                shuffledList[0].SetActive(true);
            }
            else
            {
                Debug.LogWarning("No possible objects to activate.");
            }
        }


    }

}
