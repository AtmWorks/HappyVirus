using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnProcedural : MonoBehaviour
{
    
    public enum spawnOrientation
    {
        R,
        L,
        T,
        B
    }
    public GameObject mapToSpawn;

    // Propiedad para almacenar el tipo actual
    public spawnOrientation orientation;

}
