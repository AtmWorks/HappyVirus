using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class meatBallHP : MonoBehaviour
{
    public GameObject explosion;
    public GameObject reward;
    //Sprites de enfermo

    public GameObject parent;
    public SpriteRenderer bodyrend;

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

        
        if (enemyHP <= 0 )
        {
            animator.SetBool("isDead", true);
            meatBall.enabled = false;
            EnemyDies();
        }

    }

    void EnemyDies()
    {
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        Instantiate(reward, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y+1, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x +1, this.gameObject.transform.position.y-1, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x -1, this.gameObject.transform.position.y + 1, 0), Quaternion.identity);
        Instantiate(explosion, new Vector3(this.gameObject.transform.position.x -1, this.gameObject.transform.position.y - 1, 0), Quaternion.identity);

            bodyrend.material = whiteMat;
            Destroy(parent);

    }
}