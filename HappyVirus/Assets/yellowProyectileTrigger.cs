using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yellowProyectileTrigger : MonoBehaviour
{
    public bool isOnTrigger;
    public GameObject enemy;
    public bool isSelected;
    public float enableTimer;
    private void Start()
    {
        enableTimer = 0;
        isSelected = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Enemy") || collision.gameObject.tag == ("Neutral"))
        {
            if (!isSelected && enableTimer >=1f)
            {
                isOnTrigger = true;
                enemy = collision.gameObject;
                isSelected = true;
            }
            
        }
    }
    private void Update()
    {
        enableTimer += Time.deltaTime;
    }
}
