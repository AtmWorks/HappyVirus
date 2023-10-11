using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleBlueEnemyHP : MonoBehaviour
{
    [SerializeField] public Animator animator;
    public GameObject parent;
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (animator != null)
        {
            if (collision.gameObject.tag=="Proyectil")
            {
                parent.tag = "Neutral";
                animator.SetBool("isDead", true);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
