using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteThisAfter : MonoBehaviour
{
    public float timer;
    public float desiredTime;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > desiredTime)
        { 
            Destroy(gameObject);
        }

    }
}
