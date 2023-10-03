using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class blueShooterHP : MonoBehaviour
{
    public GameObject explosion;
    public GameObject EnemyObject;
    public GameObject corpse;
    public Animator thisAnim;
    public List<SimpleFlash> flashList;
    public bool EnemyAlive;

    public Material flashMaterial;
    public float deadlyTimer;
    public bool isSpawned;

    public int enemyHP;

    public List<SpriteRenderer> renders;

    // Start is called before the first frame update
    void Start()
    {
        thisAnim.enabled = false;
        StartCoroutine(delayAnimator());
        EnemyAlive = true;
        isSpawned = true;

        deadlyTimer = 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.tag == "Proyectil")
        {
            foreach (SimpleFlash flash in flashList)
            {
                flash.Flash();
            }
            enemyHP--;
        }
    }
    void EnemyDies()
    {
        EnemyAlive = false;
        thisAnim.SetBool("isDead", true);
        thisAnim.SetBool("isShooting", false);

    }
    // Update is called once per frame
    void Update()
    {
        if (enemyHP <= 0 && EnemyAlive == true)
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            // Instantiate(corpse, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);

            EnemyDies();
        }
        if (EnemyAlive == false)
        {
            foreach (SpriteRenderer render in renders)
            {
                render.material = flashMaterial;
            }
            deadlyTimer -= Time.deltaTime;
        }
        if (deadlyTimer <= 0.5f && isSpawned)
        {
            Instantiate(corpse, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            isSpawned = false;
        }
        if (deadlyTimer <= 0f)
        {
            Destroy(EnemyObject);
        }
    }
    IEnumerator delayAnimator()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 0.9f));
        thisAnim.enabled = true;
    }
}
