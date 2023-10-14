using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;
using UnityEngine.EventSystems;

public class cloneMovement : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D thisRb;
    public float maxDistance;
    public float minDistance;
    public float maxSpeed;
    public float inmuneTime;
    public Animator animator;
    public List<SimpleFlash> flashList;
    public ParticleSystem hitParticles;
    public int cloneHP;
    public GameObject spriteFlip;
    public GameObject explosion;


    private void Start()
    {
        inmuneTime = 1;
        thisRb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Damage")
        {
            if (inmuneTime <= 0)
            {
                StartCoroutine(GetDmg());
                inmuneTime = 1.5f;
            }
        }
    }

    IEnumerator GetDmg()
    {
        cloneHP--;
        animator.SetBool("CreatingEgg", true);
        hitParticles.Play();
        StartCoroutine(flashDMG());
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("CreatingEgg", false);


    }

    IEnumerator flashDMG()
    {
        foreach (SimpleFlash flash in flashList)
        {
            flash.Flash();
        }
        yield return null;
    }
    private void Update()
    {
        inmuneTime -= Time.deltaTime;
        if(cloneHP<=0)
        {
            Instantiate(explosion, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            cloneHP = 3;
            this.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        followBehaviour();
    }



    void followBehaviour()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (player.transform.position.x > transform.position.x)
            {
                Vector3 newScale = spriteFlip.transform.localScale;
                newScale.x = Mathf.Abs(newScale.x); // Garantiza que la escala sea positiva
                spriteFlip.transform.localScale = newScale;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                Vector3 newScale = spriteFlip.transform.localScale;
                newScale.x = -Mathf.Abs(newScale.x); // Garantiza que la escala sea negativa
                spriteFlip.transform.localScale = newScale;
            }
            if (distanceToPlayer > maxDistance)
            {
                // Calculate the direction to move towards the player.
                Vector2 moveDirection = (player.transform.position - transform.position).normalized;

                // Calculate the speed based on maxSpeed.
                float moveSpeed = Mathf.Min(maxSpeed, distanceToPlayer);

                // Use Vector2.Lerp to gradually change the velocity.
                thisRb.velocity = Vector2.Lerp(thisRb.velocity, moveDirection * moveSpeed, Time.fixedDeltaTime * 10);
            }
            else if (distanceToPlayer < minDistance)
            {
                // Calculate the direction to move away from the player.
                Vector2 moveDirection = (transform.position - player.transform.position).normalized;

                // Calculate the speed based on maxSpeed.
                float moveSpeed = Mathf.Min(maxSpeed, distanceToPlayer);

                // Use Vector2.Lerp to gradually change the velocity.
                thisRb.velocity = Vector2.Lerp(thisRb.velocity, moveDirection * moveSpeed, Time.fixedDeltaTime * 10);
            }
            else
            {
                thisRb.velocity = Vector2.Lerp(thisRb.velocity, Vector2.zero, Time.fixedDeltaTime * 15);
                // Inside the comfort zone, stop moving

            }
        }
    }
}
