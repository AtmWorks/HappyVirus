using Cubequad.Tentacles2D;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static bool VirusFaceCheck;
    public int colorStatus = 0;
    public GameObject smokeEffect;
    public GameObject virusFace;
    public GameObject tentaclesParent;

    //EGG behavior
    public static bool isAttractingEgg;
    public GameObject createEggButtonObject;
    public GameObject followEggButtonObject;

//BUTTONS
    public UnityEngine.UI.Button faceButton;
    public UnityEngine.UI.Button tentacleButton;
    public UnityEngine.UI.Button addO2Button;
    public UnityEngine.UI.Button smokeButton;
    public UnityEngine.UI.Button shieldRingButton;
    public UnityEngine.UI.Button eggFollowSwitch;
    public UnityEngine.UI.Button smokeButtonEnable;
    public GameObject catchTentacleButton;
    public GameObject smokeButtonObject;


    // Tentáculos
    [SerializeField] private Tentacle tentacle1;
    [SerializeField] private Tentacle tentacle2;

    // Escudo
    public GameObject shieldRing;

    public SoftBody softBody;

    // UI no relacionada a movimiento
    public Image blobCircle;

    public CameraBehaviour01 mainCamera;     // sigue usándose para otras features
    //public EggCounter virusSelect;           // sigue usándose para otras features

    public bool isOnTentacleTarget;

    private void Awake()
    {
        isOnTentacleTarget = false;
    }

    void Start()
    {
        softBody = this.gameObject.GetComponent<SoftBody>();
        virusFace.gameObject.transform.localScale = new Vector3(0, 0, 0);
        growingFace = false;

        faceButton.onClick.AddListener(changeColor);
        tentacleButton.onClick.AddListener(showTentacles);
        addO2Button.onClick.AddListener(getO2);
        smokeButton.onClick.AddListener(useSmoke);
        shieldRingButton.onClick.AddListener(CrearEscudo);
        eggFollowSwitch.onClick.AddListener(switchEggAttract);
        smokeButtonEnable.onClick.AddListener(switchSmoke);

        blobCircle.fillAmount = 0f;
    }

    void changeColor()
    {
        if (colorStatus == 0) colorStatus = 1;
        else if (colorStatus == 1) colorStatus = 0;
    }

    public GameObject getMouseOverThing()
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


    private bool growingFace;

    private void growFaceOver()
    {
        if (VirusFaceCheck == false)
        {
            softBody.softBodyPosition();
            growingFace = true;
            Debug.Log("Face Growing");
            createEggButtonObject.SetActive(true);
            followEggButtonObject.SetActive(true);
            VirusFaceCheck = true;
        }
        else
        {
            Debug.Log("Not enough O2");
        }

        if (blobCircle.fillAmount >= 1.0f)
        {
            softBody.softBodyPosition();
        }
    }

    void showTentacles()
    {
        if (tentaclesParent.activeSelf == false)
        {
            tentaclesParent.SetActive(true);
            catchTentacleButton.SetActive(true);
        }
        else
        {
            tentaclesParent.SetActive(false);
            catchTentacleButton.SetActive(false);
        }
    }

    void getO2()
    {
        PlayerStatics.maxO2counter = 100;
        PlayerStatics.O2counter = PlayerStatics.maxO2counter;
    }

    public void useSmoke()
    {
        PlayerAnimator.GetDmg = true;
        StartCoroutine(spawnSmoke());
    }

    IEnumerator spawnSmoke()
    {
        GameObject smokeInstance = Instantiate(smokeEffect, transform.position, Quaternion.identity);
        smokeInstance.transform.SetParent(transform);
        yield return new WaitForSeconds(1.5f);
        PlayerAnimator.GetDmg = false;
    }

    void switchEggAttract()
    {
        if (isAttractingEgg) isAttractingEgg = false;
        else isAttractingEgg = true;
    }

    void switchSmoke()
    {
        if (smokeButtonObject.activeSelf == true)
            smokeButtonObject.SetActive(false);
        else
            smokeButtonObject.SetActive(true);
    }

    void CrearEscudo()
    {
        GameObject copia = Instantiate(shieldRing, transform.position, transform.rotation, transform);
        copia.SetActive(true);
    }

    void FixedUpdate()
    {
        if (colorStatus != PlayerStatics.colorState)
        {
            PlayerStatics.colorState = colorStatus;
        }

        // Mecánica de cambiar de forma (no movimiento)
        if (growingFace == true)
        {
            virusFace.gameObject.transform.localScale += new Vector3(1.5f * Time.deltaTime, 1.5f * Time.deltaTime, 1.5f * Time.deltaTime);
        }
        if (virusFace.gameObject.transform.localScale.x >= 1 && virusFace.gameObject.transform.localScale.y >= 1)
        {
            virusFace.gameObject.transform.localScale = new Vector3(1, 1, 1);
            growingFace = false;
        }

        if (PlayerStatics.PlayerHP > 1 && !VirusFaceCheck)
        {
            growFaceOver();
        }

        if (PlayerStatics.PlayerHP <= 1 && VirusFaceCheck == true)
        {
            virusFace.gameObject.transform.localScale = new Vector3(0, 0, 0);
            tentaclesParent.SetActive(false);
            catchTentacleButton.SetActive(false);
            createEggButtonObject.SetActive(false);
            followEggButtonObject.SetActive(false);
            VirusFaceCheck = false;
        }

        // Atraer huevos al player (no movimiento)
        if (isAttractingEgg && PlayerAnimator.IsShooting == false)
            EggAttraction.isAbsorbing = true;
        else
            EggAttraction.isAbsorbing = false;

    }
}
