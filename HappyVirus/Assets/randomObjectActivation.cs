using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class randomObjectActivation : MonoBehaviour
{
    public List<GameObject> baseLvl1 = new List<GameObject>();
    public List<GameObject> baseLvl2 = new List<GameObject>();
    public List<GameObject> yellowTransition = new List<GameObject>();
    public List<GameObject> spawnsList = new List<GameObject>();
    public List<int> transitionsInOrder = new List<int>();



    void Start()
    {
        foreach (GameObject obj in baseLvl1)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in baseLvl2)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in yellowTransition)
        {
            obj.SetActive(false);
        }
        ActivateRandomObject();
    }

    void ActivateRandomObject()
    {
        //para rojo : 0, 1
        //para transicion de rojo a amarillo : 2
        //para amarillo : 3, 4

        if (proceduralBehaviour.difficulty == 0 || proceduralBehaviour.difficulty == 3)
        {
            if (baseLvl1.Count > 0)
            {
                // Cambia la semilla para la generación de números aleatorios
        Random.InitState(System.DateTime.Now.Millisecond);

        // Baraja la lista baseLvl1
        baseLvl1 = baseLvl1.OrderBy(x => Random.Range(0, 50)).ToList();

                // Activa el primer objeto de la lista barajada
                baseLvl1[0].SetActive(true);
            }
            else
            {
                Debug.LogWarning("No possible objects to activate.");
            }
        }

        //red complex
        else if (proceduralBehaviour.difficulty == 1 || proceduralBehaviour.difficulty == 4)
        {
            if (baseLvl2.Count > 0)
            {
                // Baraja la lista baseLvl1
                List<GameObject> shuffledList = baseLvl2.OrderBy(x => Random.Range(0, baseLvl2.Count - 1)).ToList();

                // Activa el primer objeto de la lista barajada
                shuffledList[0].SetActive(true);
            }
            else
            {
                Debug.LogWarning("No possible objects to activate.");
            }
        }

        else if (proceduralBehaviour.difficulty == 2)
        {
            if (yellowTransition.Count > 0)
            {

                List<GameObject> shuffledList = yellowTransition.OrderBy(x => Random.value).ToList();
                List<GameObject> finalList = new List<GameObject>();
                string orderToBan = proceduralBehaviour.lastOrientation;
                foreach (GameObject shuffledObj in shuffledList)
                {
                    if (!shuffledObj.name.Contains(orderToBan)) { finalList.Add(shuffledObj); break;}
                }

                List<string> orientations = new List<string> { "L", "R", "T", "B" };

                foreach (string orientation in orientations)
                {
                    if (finalList[0].name.Contains(orientation))
                    {
                        foreach (GameObject spawn in spawnsList)
                        {

                            if (spawn.name.Contains(orientation))
                            {
                                spawnProcedural spawnComponent = spawn.GetComponent<spawnProcedural>();
                                if (spawnComponent != null)
                                {
                                    if (spawnComponent.mapToSpawn == null)
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
                finalList[0].SetActive(true);
            }
            else
            {
                if (baseLvl2.Count > 0)
                {
                    // Baraja la lista baseLvl1
                    List<GameObject> shuffledList = baseLvl2.OrderBy(x => Random.Range(0, baseLvl2.Count - 1)).ToList();

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

}
