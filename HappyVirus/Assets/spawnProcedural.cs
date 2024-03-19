using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnProcedural : MonoBehaviour
{
    public proceduralBehaviour proceduralMap;
    public GameObject mapToSpawn;
    public spawnOrientation orientation;

    private void Start()
    {
        proceduralMap = GameObject.Find("Procedural map").GetComponent<proceduralBehaviour>();

        if (proceduralMap == null)
        {
            Debug.LogError("Procedural map object not found in the scene.");
        }
    }
    public enum spawnOrientation
    {
        R,
        L,
        T,
        B
    }
   
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (mapToSpawn != null)
            {
                proceduralMap.proceduralMap(orientation, mapToSpawn);
            }
            else
            {
                proceduralMap.proceduralMap(orientation);
            }
        }
    }

}
