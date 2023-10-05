using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFire : MonoBehaviour {

    public static int shootMode;
    public GameObject firePoint;
    public GameObject firePoint2;
    public List<GameObject> vfx = new List<GameObject>();

    public bool isLayingEgg;
    public isButtonPressed EggButton;


    private GameObject effectToSpawn;
    private GameObject effectToSpawn1;
    private float timeToFire = 0;

    public bool isOnPC;
    public bool isOnMobile;

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
        isLayingEgg = EggButton.buttonPressed;

        if (isOnPC)
        {
            //-MOUSE SHOOT-    
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
            //To fire// SHOOT  MODE 1

            if (Input.GetMouseButton(0))
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
                    
                    effectToSpawn = vfx[3];
                    timeToFire = Time.time + 1;
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

        }


        if (isOnMobile)
        {
            //TODO: Lo mismo que se ejecuta dentro del if (isOnPC) pero no cogera como referencia la posicion del cursor sinoque en su lugar cogera como referencia el joystick "rightJoy". Cuando rightJoy este apuntando a una dirección (tiene que estar en el valor máximo, es decir no se activará hasta que el joystick este en alguno de sus límites, no sirve si esta a medio camino) 
            Vector2 joystickDirection = rightJoy.Direction;

            // Ensure that the joystick is at one of its extremes (near the limits).
            if (joystickDirection.magnitude >= 0.9f)
            {
                float angle = Mathf.Atan2(joystickDirection.y, joystickDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
                PlayerAnimator.IsShooting = true;


                // To fire - SHOOT MODE 1
                if (shootMode == 1 && PlayerStatics.O2counter >= 1 && Time.time >= timeToFire)
                {
                    PlayerStatics.O2counter--;
                    effectToSpawn = vfx[0];
                    timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil01>().fireRate;
                    SpawnVFX();
                }
                // Add similar conditions for other shoot modes (2, 3, and 4) here.
                if (shootMode == 2 && PlayerStatics.O2counter >= 1 && Time.time >= timeToFire)
                {
                    PlayerStatics.O2counter--;
                    effectToSpawn = vfx[2];
                    timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil02>().fireRate;
                    SpawnVFX();

                }
                if (shootMode == 3 && PlayerStatics.O2counter >= 3 && Time.time >= timeToFire) //DISPARO TRIPLE
                {
                    PlayerStatics.O2counter-=3;
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
        }


        //TO CREATE AN EGG
        if (Input.GetKey("space")|| isLayingEgg)
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
    void SpawnVFX (Quaternion rotation)
    {
        GameObject vfx;

        if (firePoint != null)
        { vfx = Instantiate(effectToSpawn, firePoint.transform.position, rotation); }

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
