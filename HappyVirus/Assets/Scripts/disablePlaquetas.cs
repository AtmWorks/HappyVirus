using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disablePlaquetas : MonoBehaviour
{
    public GameObject explosion;

    public SpawnBehaviour spawn;
    //public bool isInfected;
    // Start is called before the first frame update
    void Start()
    {
        //isInfected = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (spawn.isInfected == true ) 
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);

            this.gameObject.SetActive(false);
        
        }
        //{ isInfected = true; }
    }
}
