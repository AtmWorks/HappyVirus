using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleBlueEnemyBehaviour : MonoBehaviour
{
    public bool isDead;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        animator = gameObject.GetComponent<Animator>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Virus"|| collision.gameObject.tag == "Damage"|| collision.gameObject.tag == "Enemy")
        {
            animator.SetBool("isExploding", true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            Destroy(this.gameObject);
        }
    }
}
