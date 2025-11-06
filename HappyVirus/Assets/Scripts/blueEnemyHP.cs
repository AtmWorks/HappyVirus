using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class blueEnemyHP : MonoBehaviour
{
    public int enemyHP;
    public GameObject explosion;
    public GameObject corpse;
    public GameObject EnemyObject;
    public List<SimpleFlash> flashList;
    public BlueEnemyBehavior parentTag;

    // Optimizaciones internas (no renombran tus variables públicas)
    private Transform _tr;
    private bool _deathProcessed;

    void Start()
    {
        _tr = transform; // cache del transform
    }

    void EnemyDies()
    {
        this.gameObject.tag = "invisible";
        if (parentTag != null)
        {
            parentTag.isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collisionTrig)
    {
        if (collisionTrig.gameObject.CompareTag("Proyectil"))
        {
            StartCoroutine(flashDMG());
            enemyHP--;
        }
    }

    private IEnumerator flashDMG()
    {
        if (flashList != null)
        {
            foreach (SimpleFlash flash in flashList)
            {
                if (flash != null && flash.gameObject.activeSelf)
                {
                    flash.Flash(0.15f);
                }
            }
        }
        yield return null;
    }

    void FixedUpdate()
    {
        // Evita ejecutar la muerte múltiples veces por frame
        if (_deathProcessed || enemyHP > 0)
            return;

        _deathProcessed = true;

        // Marca estado de muerte (tag y parent)
        EnemyDies();

        // Cache de posición (z forzada a 0 como antes)
        Vector3 pos = _tr.position;
        pos.z = 0f;

        // Mantiene 3 explosiones, pero sin repetir código
        for (int i = 0; i < 3; i++)
        {
            Instantiate(explosion, pos, Quaternion.identity);
        }
        Instantiate(corpse, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
        // Destruye el objeto del enemigo como antes
        if (EnemyObject != null)
        {
            Destroy(EnemyObject);
        }
    }
}
