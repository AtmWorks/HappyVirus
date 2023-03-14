using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour {


    private Rigidbody2D rb;
    private Transform moveTo;
    public float ChaseSpeed;
    public bool isChasing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isChasing = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Virus" && isChasing == false)
        {
            isChasing = true;
            moveTo = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Virus" && isChasing == true)
        {
            isChasing = false;
        }
    }
    void FixedUpdate()
    {
        if (isChasing == true)
        {
            Vector2 movement = Vector2.MoveTowards(rb.position, moveTo.position, ChaseSpeed * Time.deltaTime);
            rb.MovePosition(movement);
        }
    }
}
