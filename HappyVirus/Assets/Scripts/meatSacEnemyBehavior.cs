using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;
public class meatSacEnemyBehavior : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int hp = 3;

    [Header("Rango de activación")]
    [SerializeField] private float awakeDistance = 5f;

    [Header("Referencias (asignar en Inspector)")]
    [SerializeField] private Animator animator;           // Debe asignarse en el inspector
    [SerializeField] private GameObject player;           // Si es null, se busca en Start por tag "Player"
    [SerializeField] private GameObject explosionEffect;  // Prefab a instanciar al morir
    [SerializeField] private GameObject loot;             // Prefab a instanciar al morir
    [SerializeField] private Transform spawnPoint;        // Punto de spawn para explosion/loot (si es null, usa this.transform)
    [SerializeField] private GameObject objectToDestroy;  // Objeto a destruir al morir

    [Header("Animator Params")]
    [SerializeField] private string isShootingParam = "isShooting";
    [SerializeField]private List <SimpleFlash> flashList;

    // Tiempo de espera sin amenazas para dejar de esconderse

    private void Reset()
    {
        // Autocompletar referencias comunes
        if (!animator) animator = GetComponent<Animator>();
        if (!spawnPoint) spawnPoint = transform;
        if (!objectToDestroy) objectToDestroy = gameObject;
    }

    private void Start()
    {
        // Buscar Player si no está asignado
        if (player == null)
        {
            GameObject found = GameObject.FindWithTag("Player");
            if (found != null) player = found;
        }

        if (!animator)
        {
            animator = GetComponent<Animator>();
            if (!animator)
                Debug.LogWarning($"[{name}] No hay Animator asignado. Asigna uno en el inspector.");
        }

        if (!spawnPoint) spawnPoint = transform;
        if (!objectToDestroy) objectToDestroy = gameObject;
    }

    private void Update()
    {
        HandleAwakeDistance();
    }

    // --------------------------------------------------------------------
    // Lógica de distancia con el Player
    // --------------------------------------------------------------------
    private void HandleAwakeDistance()
    {
        if (!player || !animator) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > awakeDistance)
        {
            animator.SetBool(isShootingParam, false);
        }
        else
        {
            animator.SetBool(isShootingParam, true);
        }
    }

    // --------------------------------------------------------------------
    // Daño por colisión física (no trigger)
    // --------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Proyectil"))
        {
            GetDamage(); // daño por defecto = 1
        }
    }


    // --------------------------------------------------------------------
    // Lógica de daño y destrucción
    // --------------------------------------------------------------------
    public void GetDamage(int amount = 1)
    {
        hp -= Mathf.Max(1, amount);
        StartCoroutine(flashDMG());
        if (hp <= 0)
        {
            Die();
        }
    }

        IEnumerator flashDMG()
    {
        foreach (SimpleFlash flash in flashList)
        {
            if (flash.gameObject.activeSelf == true)
            {
                flash.Flash(0.25f);
            }
        }
        yield return null;
    }

    private void Die()
    {
        Vector3 pos = spawnPoint ? spawnPoint.position : transform.position;
        Quaternion rot = spawnPoint ? spawnPoint.rotation : Quaternion.identity;

        if (explosionEffect) Instantiate(explosionEffect, pos, rot);
        if (loot) Instantiate(loot, pos, rot);

        if (objectToDestroy)
            Destroy(objectToDestroy);
        else
            Destroy(gameObject);
    }

    public int CurrentHP => hp;

    // --------------------------------------------------------------------
    // Gizmo para visualizar awakeDistance
    // --------------------------------------------------------------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, awakeDistance);
    }
}
