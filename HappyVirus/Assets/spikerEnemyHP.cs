using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class spikerEnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp;
    public List<SimpleFlash> FlashList;
    public spikerEnemyBehaviour parent;
    public Material whiteMaterial;
    public Animator animator;
    public bool isDead;
    public Collider2D thisCollider;
    void Start()
    {
        hp = 8;
        isDead = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Proyectil")
        {
            if (hp > 0)
            {
                foreach (SimpleFlash flash in FlashList)
                {
                    flash.Flash();
                }
                hp--;
            }

        }
        

    }
    private void Update()
    {
       if (hp <= 0 && !isDead)
       {
            StartCoroutine(allWhite());
       }
       if (isDead)
        {
            Destroy(parent.gameObject);
        }
    }

    IEnumerator allWhite()
    {
        foreach(SimpleFlash flash in FlashList)
        {
            GameObject objetToWhite = flash.gameObject;
            SpriteRenderer renderer = objetToWhite.GetComponent<SpriteRenderer>();
            renderer.material = whiteMaterial;
        }
        thisCollider.enabled = false;
        animator.SetBool("isDead", true);
        yield return null;
    }
}
