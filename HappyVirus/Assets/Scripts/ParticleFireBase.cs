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
    public GameObject rightJoyObject;

    public float offset;
    void Start()
    {
        shootMode = 1;
        //TODO rightJoy is an object called Variable Joystick (Shoot)
        rightJoyObject = GameObject.Find("Variable Joystick (Shoot)");
        rightJoy = rightJoyObject.GetComponent<Joystick>();


    }

    void FixedUpdate()
    {
        Vector2 target = rightJoy.Direction;
        if (target.magnitude >= 0.9f)
        {
            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
            Shoot();
        }
        else
        {
            thisAnimator.SetBool("IsShooting", false);
        }
    }
        public void Shoot()
        {
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
