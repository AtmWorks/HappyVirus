using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class meatBallHP : MonoBehaviour
{
    public GameObject explosion;
    //Sprites de enfermo

    public GameObject parent;
    public SpriteRenderer bodyrend;
    public Sprite normalBody, infectedBody, infectedPlusBody,deadBody;

    //public GameObject EnemyObject;
    public Animator animator;
    public bool Alive;
    public int enemyHP;
    public meatBall meatBall;
    public Material whiteMat;
    [SerializeField] private List<SimpleFlash> flashList;

    // Use this for initialization
    void Start()
    {
        Alive = true;
        enemyHP = 12;
    }


    IEnumerator flashDMG()
    {

        foreach (SimpleFlash flash in flashList)
        {
            flash.Flash();

        }
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.tag == "Proyectil")
        {

            enemyHP--; 
            StartCoroutine(flashDMG());
        }
    }

    void Update()
    {

        if (enemyHP <= 9 && enemyHP> 6) { 
            bodyrend.sprite = infectedBody;
        }
        if (enemyHP <= 6 && enemyHP > 3)  
        {
            bodyrend.sprite = infectedPlusBody;
            animator.SetBool("isInfected", true);

        }

        if (enemyHP <= 3 )
        {
            bodyrend.sprite = deadBody;
            animator.SetBool("isVeryInfected", true);

        }
        if (enemyHP <= 0 )
        {
            animator.SetBool("isDead", true);
            meatBall.enabled = false;
            StartCoroutine(EnemyDies());
        }

    }

    IEnumerator EnemyDies()
    {

            bodyrend.material = whiteMat;
            yield return new WaitForSeconds(2.5f);
            Destroy(parent);

    }
}