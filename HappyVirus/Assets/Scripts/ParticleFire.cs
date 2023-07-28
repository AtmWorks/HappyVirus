using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFire : MonoBehaviour {

    public static int shootMode;
    public GameObject firePoint;
    public GameObject firePoint2;
    public List<GameObject> vfx = new List<GameObject>();

    private GameObject effectToSpawn;
    private GameObject effectToSpawn1;
    private float timeToFire = 0;

    public Joystick rightJoy;

    private float currentEggTime = 0f;
    private float startingEggTime = 2f;

    public float offset;

	void Start ()
    {
        shootMode = 1;
        currentEggTime = startingEggTime;
	}

    void FixedUpdate()
    {

        //JOYSTICK FIRE
        /*
        Vector3 joyRotation = (Vector3.up * rightJoy.Horizontal + Vector3.left * rightJoy.Vertical);
        if (rightJoy.Horizontal != 0 || rightJoy.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, joyRotation);
            if(Time.time >= timeToFire && PlayerStatics.O2counter >= 1 && PlayerStatics.O2counter >= 1)
            {

                if (shootMode == 1 && PlayerStatics.O2counter >= 1)
                {
                    PlayerAnimator.IsShooting = true;
                    PlayerStatics.O2counter--;
                    effectToSpawn = vfx[0];
                    timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil01>().fireRate;
                    SpawnVFX();


                }

            }

        }
        else { PlayerAnimator.IsShooting = false; }
        */

        
            //-MOUSE SHOOT-
            //To face Mouse
            
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        //To fire// SHOOT  MODE 1
        
        if (Input.GetMouseButton(0)  )
        {
            PlayerAnimator.IsShooting = true;

            if (shootMode == 1 && PlayerStatics.O2counter >= 1 && Time.time >= timeToFire)
            {
                PlayerStatics.O2counter--;
                effectToSpawn = vfx[0];
                timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil01>().fireRate;
                SpawnVFX();

            }
            
            if (shootMode == 2 && PlayerStatics.O2counter >= 1)
            {
                PlayerStatics.O2counter--;
                effectToSpawn = vfx[2];
                timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil02>().fireRate;
                SpawnVFX();


            }
            if (shootMode == 3 && PlayerStatics.O2counter >= 1)
            {
                PlayerStatics.O2counter--;
                effectToSpawn = vfx[3];
                timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Bomb01>().fireRate;
                SpawnVFX();


            }
            if (shootMode == 4 && PlayerStatics.O2counter >= 1)
            {
                PlayerStatics.O2counter--;
                effectToSpawn = vfx[4];
                timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Bomb02>().fireRate;
                SpawnVFX();

            }

        }
        else
        {
            PlayerAnimator.IsShooting = false;
        }

        //To fire// SHOOT  MODE 2


        //TO CREATE AN EGG
        if (Input.GetKey("space"))
        {
            PlayerAnimator.IsCreatingEgg = true;

            if (PlayerAnimator.IsShooting == false && PlayerStatics.O2counter >= 3 && PlayerMovement.VirusFaceCheck == true)
            {
                currentEggTime -= 1 * Time.deltaTime;
            }
            else
            {
                currentEggTime = startingEggTime;
                Debug.Log("SE DEBERIA HABER CAMBIADO");
            }
        }
        else { PlayerAnimator.IsCreatingEgg = false; }


        if (currentEggTime <= 0)
        {

            effectToSpawn = vfx[1];
            SpawnEgg();
            PlayerStatics.EggsCounterPubl++;
            PlayerStatics.O2counter -= 3;
            currentEggTime = startingEggTime;
        }



    }

    //Function to spawn Visual effects from a list. 
    void SpawnVFX ()
    {
        GameObject vfx;

        if (firePoint != null)
        { vfx = Instantiate(effectToSpawn, firePoint.transform.position, transform.rotation); }

        else
        { Debug.Log("No Fire Point"); }
    }

    void SpawnEgg()
    {
        GameObject vfx;

        if (firePoint != null)
        { vfx = Instantiate(effectToSpawn, firePoint2.transform.position, transform.rotation); }

        else
        { Debug.Log("No Fire Point"); }

    }
		
}
