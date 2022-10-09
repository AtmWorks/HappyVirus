using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossDMG : MonoBehaviour
{

    public int bossHP;
    // Start is called before the first frame update
    void Start()
    {
        bossHP = 50;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Proyectil")
        {
            bossHP--;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (bossHP <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
