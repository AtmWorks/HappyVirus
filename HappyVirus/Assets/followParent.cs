using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followParent : MonoBehaviour
{
    public GameObject parentFollow; // Objeto al que queremos seguir
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (parentFollow != null)
        {
            // Obtén la posición del parentFollow
            Vector2 targetPosition = parentFollow.transform.position;

            // Mueve el objeto usando el Rigidbody2D
            rb2d.MovePosition(targetPosition);
        }
    }
}
