using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static bool VirusFaceCheck;
    //public bool VirusFaceCheck;
    public GameObject VirusFace;
    public float speed;
    private Rigidbody2D rig;
    private bool growingFace;
    //
    public Image Stamina;
    public bool coolingDown;
    public float waitTime = 1.0f;
    

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Start ()
    {
        VirusFace.gameObject.transform.localScale = new Vector3(0, 0, 0);
        //VirusFaceCheck = false;
        
        waitTime = 2f;

    }
	
	private void faceGrow ()
    { VirusFace.gameObject.transform.localScale += new Vector3(1.5f *Time.deltaTime, 1.5f*Time.deltaTime,1.5f* Time.deltaTime); }

	void Update ()
    {

        if (growingFace== true  )
        {
            Debug.Log ("Face Growing");
            faceGrow(); }

        if (VirusFace.gameObject.transform.localScale.x >= 1 && VirusFace.gameObject.transform.localScale.y >= 1)
        {
                growingFace = false;
            VirusFaceCheck = true;
            VirusFace.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            VirusFaceCheck = false;
        }

        if (Input.GetKeyDown("p") && PlayerStatics.O2counter >=3 && VirusFaceCheck == false)
        {
            growingFace = true;
            //VirusFace.gameObject.transform.localScale = new Vector3 (1, 1, 1) ; 
            //VirusFace.SetActive(true);
            PlayerStatics.O2counter -= 3;
            PlayerCollision.PlayermaxHP = 10;
            PlayerCollision.PlayerHP = 10;
            PlayerStatics.VirusState = 2;


        }

        if (PlayerCollision.PlayerHP <= 3 && VirusFaceCheck == true)
        {
            //VirusFace.SetActive(false);
            PlayerStatics.VirusState = 1;
            VirusFace.gameObject.transform.localScale = new Vector3(0, 0, 0);

            PlayerCollision.PlayermaxHP =3;
            PlayerCollision.PlayerHP = 3;
        }
        //rig.constraints = RigidbodyConstraints2D.FreezeRotation

        //ATRAER HUEVOS AL PLAYER//
        if (Input.GetMouseButton(1) && PlayerAnimator.IsShooting == false)
        {
            EggAttraction.isAbsorbing = true;

        }
        else
        {
            EggAttraction.isAbsorbing = false;
        }

        //ATRAER HUEVOS AL NEXO//
        if (Input.GetKeyDown("f") && PlayerAnimator.IsShooting == false && EggCounter.NexusFull == false)
        {
            EggAttraction.isAtracting = true;

        }

        //SPRINT
        if (coolingDown == true )
        {
            //Reduce fill amount over 30 seconds
            Stamina.fillAmount -= 2.0f / waitTime * Time.deltaTime;
        }
        else
        {
            Stamina.fillAmount += 1.0f / waitTime * Time.deltaTime;
        }

       

        if (Input.GetKey("left shift") && Stamina.fillAmount >= 0.05f)
        {

            coolingDown = true;
            speed = 10;

        }
        else
        {
            coolingDown = false;
            speed = 6;
        }


        //MOVIMIENTO//
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);
        rig.velocity = movement * speed;

        //CONGELA PLAYER//

        if (PlayerAnimator.IsShooting == true && PlayerStatics.VirusState == 2 || PlayerAnimator.IsCreatingEgg == true && PlayerStatics.VirusState == 2)
        {
           // rig.constraints = RigidbodyConstraints2D.None;
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
            //rig.constraints = RigidbodyConstraints2D.
        }
        else
        {
            rig.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            rig.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }

        //MODOS DE DISPARO//
        if (Input.GetKeyDown("1"))
        {          
            ParticleFire.shootMode = 1;
            Debug.Log("I SWITCHED TO MODE1");            
        }
         if (Input.GetKeyDown("2"))
        {          
            ParticleFire.shootMode = 2;
            Debug.Log("I SWITCHED TO MODE2");            
        }
        if (Input.GetKeyDown("3"))
        {          
            ParticleFire.shootMode = 3;
            Debug.Log("I SWITCHED TO MODE3");            
        }
        if (Input.GetKeyDown("4"))
        {          
            ParticleFire.shootMode = 4;
            Debug.Log("I SWITCHED TO MODE4");            
        }
        
    }
}
