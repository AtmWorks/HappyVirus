using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyHp : MonoBehaviour
{

    public GameObject explosion;
    public GameObject EnemyObject;
    public GameObject corpse;
    public Material flashMaterial;
    public List<SimpleFlash> flashList;

    public int enemyHP;

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

 
    void FixedUpdate()
    {

        if (enemyHP <= 0)
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Instantiate(corpse, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(EnemyObject);

        }

    }
}