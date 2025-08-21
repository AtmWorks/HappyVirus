using System.Collections;
using UnityEngine;

public class disablePlaquetas : MonoBehaviour
{
    public GameObject explosion;
    public GameObject O2;
    public GameObject parent; // se usará para obtener el abuelo
    public bool isInfected;
    public bool gotTouch;
    public Animator anim;

    void Start()
    {
        isInfected = false;
        gotTouch = false;
    }

    void explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Instantiate(O2, transform.position, Quaternion.identity);

        // Destruir al abuelo en lugar del padre
        if (parent != null && parent.transform.parent != null)
        {
            Destroy(parent.transform.parent.gameObject);
        }
        else
        {
            Destroy(parent); // fallback si no hay abuelo
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Virus") || collision.gameObject.CompareTag("Proyectil"))
        {
            anim.SetBool("infection", true);
            gotTouch = true;
            StartCoroutine(HandleInfection());
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Proyectil"))
        {
            anim.SetBool("infection", true);
            gotTouch = true;
            StartCoroutine(HandleInfection());
        }
    }

    private IEnumerator HandleInfection()
    {
        // esperar 1 segundo desde el primer contacto
        yield return new WaitForSeconds(1f);

        // confirmar infección y explotar
        if (gotTouch && !isInfected) // evitar múltiples explosiones
        {
            isInfected = true;
            explode();
        }
    }
}
