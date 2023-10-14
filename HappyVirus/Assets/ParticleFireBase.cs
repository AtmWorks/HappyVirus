using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFireBase : MonoBehaviour
{

    public static int shootMode;
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public Animator thisAnimator;

    private GameObject effectToSpawn;
    private float timeToFire = 0;

    public bool isOnPC;
    public bool isOnMobile;

    public Joystick rightJoy;

    public float offset;

    void Start()
    {
        shootMode = 1;

    }

    void FixedUpdate()
    {
        
        if (isOnMobile)
        {
            //TODO: Lo mismo que se ejecuta dentro del if (isOnPC) pero no cogera como referencia la posicion del cursor sinoque en su lugar cogera como referencia el joystick "rightJoy". Cuando rightJoy este apuntando a una dirección (tiene que estar en el valor máximo, es decir no se activará hasta que el joystick este en alguno de sus límites, no sirve si esta a medio camino) 
            Vector2 joystickDirection = rightJoy.Direction;

            // Ensure that the joystick is at one of its extremes (near the limits).
            if (joystickDirection.magnitude >= 0.9f)
            {
                float angle = Mathf.Atan2(joystickDirection.y, joystickDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
                // To fire - SHOOT MODE 1
                if (shootMode == 1 && PlayerStatics.O2counter >= 1 && Time.time >= timeToFire)
                {
                    Debug.Log("EL CLON DEBERIA DISPARAR");
                    thisAnimator.SetBool("IsShooting", true);
                    PlayerStatics.O2counter--;
                    effectToSpawn = vfx[0];
                    if (effectToSpawn.GetComponent<Proyectil01>() != null)
                    {
                        timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil01>().fireRate;
                        SpawnVFX();

                    }
                    else
                    {
                        timeToFire = Time.time + 1;
                        SpawnVFX();

                    }
                }
                // Add similar conditions for other shoot modes (2, 3, and 4) here.
                if (shootMode == 2 && PlayerStatics.O2counter >= 1 && Time.time >= timeToFire)
                {
                    thisAnimator.SetBool("IsShooting", true);
                    PlayerStatics.O2counter--;
                    effectToSpawn = vfx[2];
                    timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil02>().fireRate;
                    SpawnVFX();

                }
                if (shootMode == 3 && PlayerStatics.O2counter >= 3 && Time.time >= timeToFire) //DISPARO TRIPLE
                {
                    thisAnimator.SetBool("IsShooting", true);
                    PlayerStatics.O2counter -= 3;
                    effectToSpawn = vfx[0];
                    timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil01>().fireRate;
                    SpawnVFX(transform.rotation);
                    Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 0, 15);
                    SpawnVFX(newRotation);
                    newRotation = transform.rotation * Quaternion.Euler(0, 0, -15);
                    SpawnVFX(newRotation);
                }
                if (shootMode == 4 && PlayerStatics.O2counter >= 1)
                {
                    thisAnimator.SetBool("IsShooting", true);
                    PlayerStatics.O2counter--;
                    effectToSpawn = vfx[4];
                    timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Bomb02>().fireRate;
                    SpawnVFX();

                }
                
            }
            else
            {
                thisAnimator.SetBool("IsShooting", false);
            }

        }



    }

    void SpawnVFX()
    {
        GameObject vfx;

        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, transform.rotation);
        }

        else
        { Debug.Log("No Fire Point"); }
    }

    void SpawnVFX(Quaternion rotation)
    {
        GameObject vfx;

        if (firePoint != null)
        { vfx = Instantiate(effectToSpawn, firePoint.transform.position, rotation); }

        else
        { Debug.Log("No Fire Point"); }
    }

}
