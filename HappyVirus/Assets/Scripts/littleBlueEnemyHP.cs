using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleBlueEnemyHP : MonoBehaviour
{
    [SerializeField] public Animator animator;
    public GameObject parent;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (animator != null)
        {
            if (collision.gameObject.tag=="Proyectil")
            {
                parent.tag = "invisible";
                this.gameObject.tag = "invisible";
                animator.SetBool("isDead", true);

            }
        }
    }

}
