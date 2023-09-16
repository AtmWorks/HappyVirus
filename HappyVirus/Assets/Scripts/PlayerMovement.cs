using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Tentacle;

public class PlayerMovement : MonoBehaviour
{
    public static bool VirusFaceCheck;
    //public bool VirusFaceCheck;
    public GameObject bigLight;
    public GameObject VirusFace;
    public GameObject VirusSkin;
    public GameObject VirusBody;
    public GameObject virusDoble;
    public float speed;
    private Rigidbody2D rig;
    private Rigidbody2D rigCopy;
    private bool growingFace;
    private bool outOfControl;
    public VirusAttraction control;
    public Joystick screenJoystick;


    //Tentaculos
    [SerializeField] private Tentacle tentacle1;
    [SerializeField] private Tentacle tentacle2;

    //Posicionamiento del soft body
    public GameObject blob1;
    public GameObject blob2;
    public GameObject blob3;
    public GameObject blob4;
    public GameObject blob5;
    public GameObject blob6;

    private Vector3 softPos1;
    private Vector3 softPos2;
    private Vector3 softPos3;
    private Vector3 softPos4;
    private Vector3 softPos5;
    private Vector3 softPos6;
    public Vector3 skinPos;
    public Vector3 skinScale;
    private Quaternion softRot;
    public Quaternion skinRot;


    //
    public Image Stamina;
    public bool coolingDown;
    public float waitTime = 1.0f;
    public bool resting;
    //
    public bool activedobl;
    //
    public CameraBehaviour01 mainCamera;
    public EggCounter virusSelect;

    public bool isOnTentacleTarget;

    private void Awake()
    {
        isOnTentacleTarget = false;
        rig = this.GetComponent<Rigidbody2D>();
       // rigCopy = virusDoble.GetComponent<Rigidbody2D>();
        resting = false;
    }

    void Start ()
    {
       

        //VirusBody.gameObject.SetActive(false);
        outOfControl = false;
        //VirusFaceCheck = false;
        
        waitTime = 2f;

        //guardamos posiciones del soft body

        softPos1 = blob1.transform.localPosition;
        softPos2 = blob2.transform.localPosition;
        softPos3 = blob3.transform.localPosition;
        softPos4 = blob4.transform.localPosition;
        softPos5 = blob5.transform.localPosition;
        softPos6 = blob6.transform.localPosition;
        softRot = blob1.transform.rotation;

        skinPos = VirusSkin.transform.localPosition;
        skinRot = VirusSkin.transform.localRotation;
        skinScale = VirusSkin.transform.localScale;

        VirusFace.gameObject.transform.localScale = new Vector3(0, 0, 0);
        //VirusSkin.SetActive(false);
        growingFace = false;

    }

    public GameObject getMouseOverThing()
    {
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);
            foreach (Collider2D hit in hits)
            {
                if (hit.tag == "tentacleTarget")
                {
                    isOnTentacleTarget = true;
                    return hit.gameObject;
                }
            }
            isOnTentacleTarget = false;
            return null;
        }
    }
    public GameObject GetMouseOverEnemy()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

        if (hitCollider != null && hitCollider.gameObject.tag == "tentacleTarget")
        {
            isOnTentacleTarget = true;
            return hitCollider.gameObject;

        }
        else
        {
            isOnTentacleTarget = false;
            return null;
        }
    }
    private void softBodyPosition()
    {
        VirusBody.SetActive(true);
        //VirusSkin.SetActive(true);

        blob1.transform.localPosition = softPos1;
        blob2.transform.localPosition = softPos2;
        blob3.transform.localPosition = softPos3;
        blob4.transform.localPosition = softPos4;
        blob5.transform.localPosition = softPos5;
        blob6.transform.localPosition = softPos6;
        blob1.transform.rotation = softRot;
        blob2.transform.rotation = softRot;
        blob3.transform.rotation = softRot;
        blob4.transform.rotation = softRot;
        blob5.transform.rotation = softRot;
        blob6.transform.rotation = softRot;
        VirusSkin.gameObject.transform.localPosition = skinPos;
        VirusSkin.gameObject.transform.localRotation = skinRot;
        VirusSkin.gameObject.transform.localScale = skinScale;

    }

    void FixedUpdate ()
    {

        //MECANICA DE CAMBIAR DE FORMA
        if (growingFace == true)
        {
            VirusFace.gameObject.transform.localScale += new Vector3(1.5f * Time.deltaTime, 1.5f * Time.deltaTime, 1.5f * Time.deltaTime);
        }
        if (VirusFace.gameObject.transform.localScale.x >= 1 && VirusFace.gameObject.transform.localScale.y >= 1)
        {
            VirusFace.gameObject.transform.localScale = new Vector3(1, 1, 1);
            growingFace = false;

        }
        if (Input.GetKey("p") && PlayerStatics.O2counter >=3 && VirusFaceCheck == false)
        {
            

            //VirusFace.gameObject.transform.localScale = new Vector3 (1, 1, 1) ; 
            //VirusFace.SetActive(true);
            PlayerStatics.O2counter -= 3;
            PlayerCollision.PlayermaxHP = 6;
            PlayerCollision.PlayerHP = PlayerCollision.PlayerHP+3;
            PlayerStatics.VirusState = 2;
            softBodyPosition();
            growingFace = true;
            Debug.Log("Face Growing");
            bigLight.SetActive(true);
            VirusFaceCheck = true;
            



        }
        if (PlayerCollision.PlayerHP <= 3 && VirusFaceCheck == true)
        {
            //VirusFace.SetActive(false);
            PlayerStatics.VirusState = 1;
            VirusFace.gameObject.transform.localScale = new Vector3(0, 0, 0);
            //VirusSkin.gameObject.transform.localScale = new Vector3(0, 0, 0);
            

            PlayerCollision.PlayermaxHP =3;
            PlayerCollision.PlayerHP = 3;
            bigLight.SetActive(false);
            VirusFaceCheck = false;
        }
        if (PlayerCollision.PlayerHP <= 1)
        {
            VirusBody.gameObject.SetActive(false);
        }
        //ATRAER HUEVOS AL PLAYER//
        if (Input.GetMouseButton(1) && PlayerAnimator.IsShooting == false)
        {
            EggAttraction.isAbsorbing = true;
        }
        else
        {
            EggAttraction.isAbsorbing = false;
        }
        //ATRAER HUEVOS AL NEXO DE TRANSFORMACION//
        if (Input.GetKeyDown("f") && PlayerAnimator.IsShooting == false && EggCounter.NexusFull == false && PlayerStatics.EggsCounterPubl >= 3)
        {
            EggAttraction.isAtracting = true;

        }
        //LLEVAR LOS TENTACULOS A COGER ALGO

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
        //STAMINA
        if (Stamina.fillAmount <=0.05f )
        {
            
            resting = true;
            Stamina.color = new Color32(255, 86, 69,255);
            //coolingDown = true;

        }
        else if (Stamina.fillAmount >= 0.5f)
        {
            resting = false;
            Stamina.color = new Color32(255, 246, 95,255);

        }
        if (Stamina.fillAmount >= 1f)
        {
            Stamina.color = new Color(255, 246, 95, 0f);
        } 
        else if (Stamina.fillAmount <= 1f && Stamina.fillAmount >= 0.5f)
        {
            Stamina.color = new Color32(255, 246, 95, 255);
        }
        if (Input.GetKey("left shift") && Stamina.fillAmount >= 0.05f && resting == false)
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
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        float joystickHorizontal = screenJoystick.Horizontal;
        float joystickVertical = screenJoystick.Vertical;
        Vector2 joyMovement = new Vector2(joystickHorizontal, joystickVertical);

        //para input normal
        //rig.velocity = movement * speed;
        //rig.AddForce(movement * speed * 3);


        //para joystick de screen
        rig.velocity = joyMovement * speed;

        //rig.AddForce(movement * speed);
        //Debug.Log(rig.velocity);


        //MOVIMIENTO AL DOBLE
        if (virusDoble.activeSelf == true) { activedobl = true; }
        else if (virusDoble.activeSelf == false) { activedobl = false; } //checkers

        if (Input.GetKeyDown("k") ) 
        {
            if (outOfControl == false) 
            {
                //elegir el virus que se queda
                if (virusSelect.DobleVirus.activeSelf == true)
                {
                    virusDoble = virusSelect.DobleVirus;
                }
                else if (virusSelect.DobleVirus.activeSelf == false && virusSelect.DobleVirus1.activeSelf == true)
                { virusDoble = virusSelect.DobleVirus1; }
                else if (virusSelect.DobleVirus.activeSelf == false && virusSelect.DobleVirus1.activeSelf == false && virusSelect.DobleVirus2.activeSelf == true)
                { virusDoble = virusSelect.DobleVirus2; }
                else if (virusSelect.DobleVirus.activeSelf == false && virusSelect.DobleVirus1.activeSelf == false && virusSelect.DobleVirus2.activeSelf == false && virusSelect.DobleVirus3.activeSelf == true)
                { virusDoble = virusSelect.DobleVirus3; }

                rig = virusDoble.GetComponent<Rigidbody2D>();//Cambiar el focus del rigidBody
                VirusAttraction.isOnControl = true;//para que el clon no te siga
                outOfControl = true;
                mainCamera.target = virusDoble.transform;//cambiar la camara

            }
             else if (outOfControl == true)
            {
                Debug.Log("ESTOY OUTOFCONTROL");
                rig = this.GetComponent<Rigidbody2D>();
                VirusAttraction.isOnControl = false;//para que el clon no te siga
                mainCamera.target =this.gameObject.transform;//cambiar la camara
                outOfControl = false;


            }

            //devolver el control al clon

        }


        //CONGELA PLAYER//
        if (PlayerAnimator.IsShooting == true || PlayerAnimator.IsCreatingEgg == true )
        {
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
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
