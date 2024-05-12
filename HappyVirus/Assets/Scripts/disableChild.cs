
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableChild : MonoBehaviour
{
    public GameObject Virus;
    public GameObject Child;
    public float desiredDistance = 50f;

    void Start()
    {
        // Encuentra el objeto con la etiqueta "Virus" y as�gnalo a la variable Virus
        Virus = GameObject.FindWithTag("Player");
        // Asigna el objeto hijo de este GameObject a la variable Child
        if (transform.childCount > 0)
        {
            Child = transform.GetChild(0).gameObject;
        }
    }

    void Update()
    {
        // Calcula la distancia entre Virus y el objeto hijo (Child)

        // Si la distancia entre Virus y el objeto hijo es mayor que 20, desactiva el objeto hijo
        if (Child != null)
        {
            float distance = Vector3.Distance(Virus.transform.position, Child.transform.position);

            if (distance > desiredDistance)
            {
                Child.SetActive(false);
            }
            // Si la distancia entre Virus y el objeto hijo es menor o igual a 20, activa el objeto hijo
            else
            {
                Child.SetActive(true);
            }
        }
        else { Destroy(this.gameObject); }
        
    }
}