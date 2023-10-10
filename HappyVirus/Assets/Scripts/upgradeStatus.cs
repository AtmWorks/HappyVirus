using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeStatus : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isO2;
    public bool isHP;
    public bool isStamina;
    public GameObject parent;
    private bool isDone;
    public Vector3 initialScale;
    private float timer;
    public Collider2D thisCollider;
    public GameObject Virus;
    void Start()
    {
        initialScale = parent.transform.localScale;
        isDone = false;
        timer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Virus")
        {
            Virus=collision.gameObject;
            thisCollider.enabled = false;
            isDone = true;
        }
    }

    private void Update()
    {
        if (isDone)
        {
            timer += Time.deltaTime;
            // Gradualmente reduce la escala del objeto parent
            if (timer >= 0 && timer <=1) 
            {
                parent.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, timer);
                parent.transform.position = Vector3.Lerp(parent.transform.position,Virus.transform.position, timer);
            }

            if (timer > 1)
            {
                StartCoroutine(destroyDelay());
                timer = -10;
            }
        }

    }

    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(0.02f);

        if (isO2 )
        {
            PlayerStatics.maxO2counter += 5;
            PlayerStatics.O2counter = PlayerStatics.maxO2counter;
            //popScore.popText = "+5 MAX O2!";
            //popScore.isPoping = true;

            popScore.popTextCenter = "+5 MAX O2";
            popScore.isPopingCenter = true;

            Destroy(parent);

        }
        else if (isHP)
        {
            PlayerCollision.PlayermaxHP++;
            PlayerCollision.PlayerHP = PlayerCollision.PlayermaxHP;

            //popScore.popText = "+1 HP MAX";
            //popScore.isPoping = true;
            popScore.popTextCenter = "+1 HP MAX";
            popScore.isPopingCenter = true;
            Destroy(parent);


        }
        else if (!isStamina)
        {

        }

    }

}
