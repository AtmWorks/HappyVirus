using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFire : MonoBehaviour {

    //////public AudioSource audioSource;
    //////public AudioClip sfx1, sfx2, sfx3;
    //////public List<AudioClip> sfxList;

    public static int shootMode;
    public GameObject firePoint;
    public GameObject firePoint2;
    public List<GameObject> vfx = new List<GameObject>();

    public bool isLayingEgg;
    public isButtonPressed EggButton;


    private GameObject effectToSpawn;

    private float timeToFire = 0;

    public bool isEggDisabled;



    public Joystick rightJoy;

    private float currentEggTime = 0f;
    private float startingEggTime = 2f;

    public float offset;

	void Start ()
    {

        shootMode = 1;
        currentEggTime = startingEggTime;
        isEggDisabled = true;
    }

    void FixedUpdate()
    {
        isLayingEgg = EggButton.buttonPressed;

        if (PlayerStatics.controlType == PlayerStatics.ControlScheme.PC)
        {
            if (rightJoy.gameObject.activeInHierarchy == true) { rightJoy.gameObject.SetActive(false); }

            mouseShootControl();
        }
        else if (PlayerStatics.controlType == PlayerStatics.ControlScheme.Mobile)
        {
            if (rightJoy.gameObject.activeInHierarchy == false) { rightJoy.gameObject.SetActive(true); }
            mobileShootControl();
        }


        //TO CREATE AN EGG
        if (Input.GetKey("space")|| isLayingEgg)
        {
            layEgg();
        }
        else 
        { 
            if (!isEggDisabled) 
            { 
                StartCoroutine(disableEggFace());
                isEggDisabled = true;
            } 
        }
        if (currentEggTime <= 0)
        {

            effectToSpawn = vfx[1];
            SpawnEgg();
            PlayerStatics.EggsCounterPubl++;
            PlayerStatics.O2counter -= 3;
            currentEggTime = startingEggTime;
        }

    }

    public void layEgg()
    {
        PlayerAnimator.IsCreatingEgg = true;

        PlayerMovement.isAttractingEgg = false;
        isEggDisabled = false;
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

    public void mobileShootControl()
    {
        Vector2 joystickDirection = rightJoy.Direction;
        // Ensure that the joystick is at one of its extremes (near the limits).
        if (joystickDirection.magnitude >= 0.9f)
        {
            float angle = Mathf.Atan2(joystickDirection.y, joystickDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
            PlayerAnimator.IsShooting = true;


            // To fire - SHOOT MODE 1
            if (shootMode == 1 && PlayerStatics.O2counter >= 1 && Time.time >= timeToFire)//DISPARO BASE
            {
                //////reproduceSound();
                PlayerStatics.O2counter--;
                effectToSpawn = vfx[0];
                timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil01>().fireRate;
                SpawnVFX();
            }
            // Add similar conditions for other shoot modes (2, 3, and 4) here.
            if (shootMode == 2 && PlayerStatics.O2counter >= 1 && Time.time >= timeToFire)//DISPARO DISPERSO
            {
                //////reproduceSound();
                PlayerStatics.O2counter--;
                effectToSpawn = vfx[2];
                timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil02>().fireRate;
                SpawnVFX();

            }
            if (shootMode == 3 && PlayerStatics.O2counter >= 3 && Time.time >= timeToFire) //DISPARO TRIPLE
            {
                //////reproduceSound();
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
                PlayerStatics.O2counter--;
                effectToSpawn = vfx[4];
                timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Bomb02>().fireRate;
                SpawnVFX();

            }
            if (shootMode == 5 && PlayerStatics.O2counter >= 1 && Time.time >= timeToFire) //DISPARO GUIADO
            {
                //reproduceSound();
                PlayerStatics.O2counter--;
                effectToSpawn = vfx[5];
                timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Proyectil01>().fireRate;
                SpawnVFX();

            }
        }
        else
        {
            PlayerAnimator.IsShooting = false;
        }
    }
    public void mouseShootControl() {
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
    IEnumerator disableEggFace()
    {
        yield return new WaitForSeconds(0.5f);
        if (PlayerAnimator.GetDmg == false)
        {
            PlayerAnimator.IsCreatingEgg = false;
        }
        else StartCoroutine(disableEggFace());
    }
    void SpawnVFX ()
    {
        GameObject vfx;

        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, transform.rotation);

        }

        else
        { Debug.Log("No Fire Point"); }
    }

    //////void reproduceSound()
    //////{
    //////    int clipSelected = Random.Range(0, 7);
    //////    audioSource.clip = sfxList[clipSelected];
    //////    audioSource.Play();
    //////}
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
