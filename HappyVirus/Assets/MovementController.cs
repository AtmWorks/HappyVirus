using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{

    [Header("Velocidad")]
    public float speed = 5f;
    [HideInInspector] public float baseSpeed;

    [Header("Referencias")]
    public Joystick screenJoystick;
    public isButtonPressed sprintButton;
    private Rigidbody2D rig;

    [Header("Stamina / Sprint")]
    public Image Stamina;
    public bool coolingDown;
    public float waitTime = 2f;
    public bool resting;
    private bool isSprinting;

    //[Header("Cambio de control / Cámara")]
    //public CameraBehaviour01 mainCamera;
    //public GameObject virusDoble;
    //public EggCounter virusSelect;

    [HideInInspector] public bool activedobl;
    private bool outOfControl;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        resting = false;
        isSprinting = false;
        outOfControl = false;
    }

    private void Start()
    {
        baseSpeed = speed;
    }

    private void FixedUpdate()
    {
        // Determinar sprint según esquema de control
        switch (PlayerStatics.controlType)
        {
            case PlayerStatics.ControlScheme.Mobile:
                // Tu flujo original: botón de sprint en pantalla
                isSprinting = (sprintButton != null && sprintButton.buttonPressed);
                break;
            case PlayerStatics.ControlScheme.PC:
                // En PC, sprint con Left Shift
                isSprinting = Input.GetKey(KeyCode.LeftShift);
                break;
            case PlayerStatics.ControlScheme.Controller:
                // De momento no gestionamos sprint por mando
                isSprinting = false;
                break;
        }

        // Sprint / stamina
        dashingMovement(2);

        // Movimiento según esquema
        if (PlayerStatics.controlType == PlayerStatics.ControlScheme.Mobile)
        {
            if (screenJoystick.gameObject.activeSelf == false) { screenJoystick.gameObject.SetActive(true); }
            // Movimiento con joystick en pantalla (flujo original)
            float joystickHorizontal = screenJoystick != null ? screenJoystick.Horizontal : 0f;
            float joystickVertical = screenJoystick != null ? screenJoystick.Vertical : 0f;
            Vector2 joyMovement = new Vector2(joystickHorizontal, joystickVertical);

            rig.velocity = joyMovement * speed;
        }
        else if (PlayerStatics.controlType == PlayerStatics.ControlScheme.PC)
        {
            if (screenJoystick.gameObject.activeSelf == true) { screenJoystick.gameObject.SetActive(false); }

            // Movimiento con WASD y Flechas, ignorando el joystick
            float h = 0f;
            float v = 0f;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v += 1f;

            Vector2 pcMovement = new Vector2(h, v).normalized;
            rig.velocity = pcMovement * speed;
        }
        else if (PlayerStatics.controlType == PlayerStatics.ControlScheme.Controller)
        {
            if (screenJoystick.gameObject.activeSelf == true) { screenJoystick.gameObject.SetActive(false); }

            // De momento no hacemos nada (no modificamos rig.velocity)
        }

        // Control del doble
        //if (virusDoble != null)
        //    activedobl = virusDoble.activeSelf;
        //else
        //    activedobl = false;

        //if (Input.GetKeyDown("k"))
        //{
        //    ToggleControlToClone();
        //}

        // Penalización de movimiento durante acciones (disparo/crear huevo)
        if (PlayerAnimator.IsShooting == true || PlayerAnimator.IsCreatingEgg == true)
        {
            rig.velocity = rig.velocity / 3f;
        }
        else
        {
            rig.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            rig.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }
    }

    private void dashingMovement(int mode)
    {
        if (Stamina == null) return;
        if (mode == 1)
        {
            if (coolingDown == true)
                Stamina.fillAmount -= 2.0f / waitTime * Time.deltaTime;
            else
                Stamina.fillAmount += 1.0f / waitTime * Time.deltaTime;

            if (Stamina.fillAmount <= 0.05f)
            {
                resting = true;
                Stamina.color = new Color32(255, 86, 69, 255);
            }
            else if (Stamina.fillAmount >= 0.5f)
            {
                resting = false;
                Stamina.color = new Color32(255, 246, 95, 255);
            }

            if (Stamina.fillAmount >= 1f)
                Stamina.color = new Color32(255, 246, 95, 0);
            else if (Stamina.fillAmount <= 1f && Stamina.fillAmount >= 0.5f)
                Stamina.color = new Color32(255, 246, 95, 255);

            if ((Input.GetKey("left shift") || isSprinting) && Stamina.fillAmount >= 0.05f && resting == false)
            {
                coolingDown = true;
                PlayerAnimator.IsSprinting = true;
                speed = 15f;
            }
            else
            {
                coolingDown = false;
                PlayerAnimator.IsSprinting = false;
                speed = baseSpeed;
            }
        }

        if (mode == 2)
        {
            if (coolingDown == true)
            {
                Stamina.fillAmount -= 5.0f * Time.deltaTime;
            }
            else
            {
                PlayerAnimator.IsSprinting = false;
                speed = baseSpeed;
                Stamina.fillAmount += 1.0f / waitTime * Time.deltaTime;
            }

            if (Stamina.fillAmount <= 1f)
            {
                if (Stamina.fillAmount <= 0.05f)
                    coolingDown = false;

                resting = true;
                Stamina.color = new Color32(255, 86, 69, 255);
            }

            if (Stamina.fillAmount >= 0.90f)
            {
                resting = false;
                Stamina.color = new Color32(255, 246, 95, 255);
            }

            if (Stamina.fillAmount >= 1f)
                Stamina.color = new Color32(255, 246, 95, 0);

            if ((Input.GetKey("left shift") || isSprinting) && Stamina.fillAmount >= 0.90f)
            {
                coolingDown = true;
                PlayerAnimator.IsSprinting = true;
                speed = 35f;
            }
        }
    }

    //private void ToggleControlToClone()
    //{
    //    if (virusDoble == null || mainCamera == null || virusSelect == null) return;

    //    if (outOfControl == false)
    //    {
    //        // Elegir el virus que se queda
    //        if (virusSelect.DobleVirus.activeSelf == true)
    //        {
    //            virusDoble = virusSelect.DobleVirus;
    //        }
    //        rig = virusDoble.GetComponent<Rigidbody2D>(); // Cambiar el focus del rigidBody
    //        VirusAttraction.isOnControl = true;           // para que el clon no te siga
    //        outOfControl = true;
    //        mainCamera.target = virusDoble.transform;      // cambiar la cámara
    //    }
    //    else
    //    {
    //        rig = GetComponent<Rigidbody2D>();
    //        VirusAttraction.isOnControl = false;           // para que el clon no te siga
    //        mainCamera.target = this.gameObject.transform; // cambiar la cámara
    //        outOfControl = false;
    //    }
    //}
}
