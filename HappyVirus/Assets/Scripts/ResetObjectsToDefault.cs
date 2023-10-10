using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectsToDefault : MonoBehaviour
{
    // Lista pública de GameObjects
    public List<GameObject> currentObjects;

    // Lista doble para guardar posiciones y GameObjects
    public List<Vector2> positionsList;
    //public List<GameObject> gameObjectsList;
    public List<GameObject> prefabsList;
    //public GameObject prefab;
    public bool firstTime;

    // Método para guardar posiciones y GameObjects en la lista doble en Start
    private void Start()
    {
        firstTime = true;
        makeList();
    }

    public void makeList()
    {
        positionsList = new List<Vector2>();
        //gameObjectsList = new List<GameObject>();

        foreach (GameObject obj in currentObjects)
        {
            positionsList.Add(obj.transform.position);
            // Crear un prefab temporal por cada objeto de currentObjects
            GameObject prefab = Instantiate(obj, Vector2.zero, Quaternion.identity);
            prefab.SetActive(false);
            prefab.transform.parent = transform;
            prefabsList.Add(prefab);
        }
        DestroyObjects();
        firstTime = false;
    }
    // Método para destruir objetos en la lista currentObjects y vaciar la lista
    public void DestroyObjects()
    {
        if (!firstTime) 
        {
            foreach (GameObject obj in currentObjects)
            {
                Destroy(obj);
            }

            currentObjects.Clear();
        }
        

    }

    // Método para instanciar objetos de la lista doble en las posiciones correspondientes
    // y agregarlos a la lista currentObjects
    public void InstantiateObjects()
    {
        if (!firstTime)
        {
            for (int i = 0; i < positionsList.Count; i++)
            {
                GameObject newObj = Instantiate(prefabsList[i], positionsList[i], Quaternion.identity);
                newObj.transform.parent = transform;
                newObj.SetActive(true);
                currentObjects.Add(newObj);

            }
        }
        
    }

    // Método para restablecer los objetos a su estado predeterminado
    public void ResetToDefault()
    {
        if(!firstTime)
        {
            DestroyObjects();
            InstantiateObjects();
        }


    }
}
