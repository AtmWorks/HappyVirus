using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil02 : MonoBehaviour {

    public float currentTime;
    public float speed;
    public float fireRate;

    void Start()
    {
        currentTime = 5f;
    }


    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        if (speed != 0)
        {
            transform.position += transform.right * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No Speed");
        }
        if (currentTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
