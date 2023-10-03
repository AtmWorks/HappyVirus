using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepTentacle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = spawnPoint.transform.position;
    }
}
