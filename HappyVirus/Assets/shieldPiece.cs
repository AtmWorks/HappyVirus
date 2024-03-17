using UnityEngine;
using System.Collections;

public class MovementAndDamage : MonoBehaviour
{
    public GameObject targetPoint;
    public float speed;
    public GameObject explodeFx;
    public int HP;
    public float invincibleTime;
    public float arrivalThreshold = 0.1f; // Umbral de llegada

    private Rigidbody2D rb;
    private bool isInvincible = false;
    private bool isArriving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (targetPoint != null)
        {
            Vector2 direction = (targetPoint.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, targetPoint.transform.position);

            // Comprueba si ha llegado a la posición
            if (distance <= arrivalThreshold)
            {
                isArriving = true;
                rb.velocity = Vector2.Lerp(rb.velocity, direction * 5, Time.deltaTime * 2f); // Lerp a velocidad 1
            }
            else
            {
                isArriving = false;
                rb.velocity = Vector2.Lerp(rb.velocity, direction * speed, Time.deltaTime * 0.5f); // Lerp a velocidad speed
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage") && !isInvincible)
        {
            TakeDamage();
            StartCoroutine(InvincibleDelay());
            Explode();
        }
    }

    IEnumerator InvincibleDelay()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void TakeDamage()
    {
        HP--;
        if (HP <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        if (explodeFx != null)
        {
            Instantiate(explodeFx, transform.position, Quaternion.identity);
        }
    }
}

