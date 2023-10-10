using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioAndDestroy : MonoBehaviour
{
    public static bool proyectilSound;
    private bool didReproduce;
    private AudioSource aSource;
    public AudioClip sfxHit;
    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
        proyectilSound = false; 
        bool didReproduce;

    }


    private void Update()
    {
        if (proyectilSound)
        {
            //if (!didReproduce) 
            //{
            //    didReproduce = true;
                //StartCoroutine(soundAndDestroy());
                reproduceProyectilSound();
                proyectilSound = false;
            //}
        }
    }
    void reproduceProyectilSound()
    {
        aSource.clip = sfxHit;
        aSource.Play();
    }
    //IEnumerator soundAndDestroy()
    //{
    //    reproduceSound();
    //    yield return new WaitForSeconds(2f);
    //    Destroy(this.gameObject);
    //}
}
