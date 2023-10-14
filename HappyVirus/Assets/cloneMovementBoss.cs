using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cloneMovementBoss : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D thisRb;
    public float maxDistance;
    public float minDistance;
    public float maxSpeed;


    private void Start()
    {
        thisRb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage"))
        {
        }
    }

    void FixedUpdate()
    {
        followBehaviour();
    }

    //TODO: Modifica el siguiente metodo para que ademas de seguir al player, cuando tenga un objeto Damage cerca, modifique levemente la rotacion de su direccion para intentar evitar el obstaculo, sin dejar de seguir al player pero añadiendo algunos grados en contrario a Dmg para sortearlo.

    void followBehaviour()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

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
