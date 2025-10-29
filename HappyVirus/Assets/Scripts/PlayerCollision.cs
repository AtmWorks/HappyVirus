using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {


    PlayerStatics playerStatics;
    private int lastSoundPos;
    private int lastSoundPosO2;
    public bool isSpeaker;
    public bool nextSound;
//    public AudioSource audioSource;
//    public AudioSource audioSourceSecondary;
//    public AudioClip dmgClip;
//    public List<AudioClip>sfxList;
//    public List<AudioClip>o2sfxList;
    private static bool isPlaying = false;
    public bool thisPlayed;

    public GameObject virusParent;
    public GameObject explosion;
    public GameObject spawn;
    public static float InmuneTime ;
    public static float FaceTime ;
    public static bool gotDamage;
    public spawnController spawnController;
    public PlayerMovement player;

    public GameObject virusSkin;

    // public FadeToBlack thisTeleport;
    public ParticleSystem hitParticles;

    private CameraEffectController cameraEffectController;
    public float inmunityTime = 1f;
    [SerializeField]private List <SimpleFlash> flashList;


    // public GameObject PlayerObject;

    void Start ()
    {
        playerStatics = FindFirstObjectByType<PlayerStatics>();
        cameraEffectController = FindFirstObjectByType<CameraEffectController>();

        thisPlayed = false;
        gotDamage = false;
    }

    public void PlayerDies ()
    {
        //this.gameObject.SetActive(false);
        //SPAWN:
        //virusParent.transform.position = spawn.transform.position;
        //this.transform.parent.transform.position = spawn.transform.position;
        //PlayerHP = 3;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        cameraEffectController.triggerFade(0.5f, 1f);
        // thisTeleport.teleportPlayer(true, 0.8f);
        virusSkin.SetActive(false);
        StartCoroutine(dameUnRespiro());



    }
    IEnumerator dameUnRespiro()
    {
        // Espera 1 segundo.
        yield return new WaitForSeconds(1f);
        virusSkin.SetActive(true);
        PlayerStatics.PlayerHP = PlayerStatics.PlayermaxHP;

        // player.blobCircle.fillAmount = 0;
        // player.blobCircle.color = new Color32(95, 255, 100, 255);

        spawnController.spawnProcess();

    }
    void oneDamage()
    {
        PlayerAnimator.GetDmg = true;
        PlayerStatics.inmuneTimer = 1f;
        FaceTime = 1f;
        playerStatics.substractHP(1);
        // cameraEffectsController.triggerShake();
        hitParticles.Play();
    }

    IEnumerator flashDMG()
    {

        //flashEffect.Flash();
        //flashEffectPiedra1.Flash();
        //flashEffectPiedra2.Flash();
        //flashEffectPiedra3.Flash();

        //flashEffectBoca.Flash();
        //flashEffectEyeL.Flash();
        //flashEffectEyeR.Flash();
        foreach (SimpleFlash flash in flashList)
        {
            if (flash.gameObject.activeSelf == true)
            {
                flash.Flash(0.25f);
            }
        }
        yield return null;
    }
    private void OnCollisionStay2D(Collision2D PlayerCol )
    {
        if (PlayerCol.gameObject.tag == "Damage" && PlayerStatics.inmuneTimer <= 0 && FaceTime <= 0)
        {
            //reproduceHitSound();
            // I only want one contact, so that's why I initialise it with capacity 1
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            PlayerCol.GetContacts(contacts);
            // you can get the point with this:
            var contactPoint = contacts[0].point;
            //Debug.Log(contactPoint);
            // Do something with that point


            Instantiate(explosion, new Vector3(contactPoint.x, contactPoint.y, 0), Quaternion.identity);

           // Debug.Log("ENEMY TOUCH");

            gotDamage = true;



        }
        if (PlayerCol.gameObject.tag == "Wall" || PlayerCol.gameObject.tag == "Enemy" || PlayerCol.gameObject.tag == "Neutral")
        {
            if (isSpeaker)
            {
                if (!thisPlayed)
                {
                    if (!isPlaying)
                    {
                        //reproduceBlobSound();
                        thisPlayed = true;
                    }
                }
            }


        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall"|| collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Neutral")
        {
            //StartCoroutine(enableThisSound());
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spawn")
        {
            spawn = collision.gameObject;
            Debug.Log("IM ON SPAWN");

        }
    }

    IEnumerator disableGetDmg()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerAnimator.GetDmg = false;

    }

//    void reproduceBlobSound()
//    {

//        int clipSelected = Random.Range(0, 3);
//        while (clipSelected== lastSoundPos)
//        {
//        clipSelected = Random.Range(0, 3);
//        }
//        lastSoundPos = clipSelected;
//        //audioSource.clip = sfxList[clipSelected];
//        //audioSource.Play();
//        isPlaying = true;


////        StartCoroutine(enableAudio());
//    }
//    void reproduceHitSound()
//    {
//        isPlaying = true;
//        //audioSource.clip = dmgClip;
//        //audioSource.Play();
////        StartCoroutine(enableAudio());

//    }
    //public void reproduceO2Sound()
    //{
    //    int clipSelected = Random.Range(0, 2);
    //    while (clipSelected == lastSoundPosO2)
    //    {
    //        clipSelected = Random.Range(0, 2);
    //    }
    //    lastSoundPosO2 = clipSelected;
    //    audioSourceSecondary.clip = o2sfxList[clipSelected];
    //    audioSourceSecondary.Play();
    //    //StartCoroutine(enableAudio());

    //}
    //IEnumerator enableAudio()
    //{
    //    yield return new WaitForSeconds(1f);
    //    isPlaying = false;

    //}
    //IEnumerator enableThisSound()
    //{
    //    yield return new WaitForSeconds(2f);
    //    thisPlayed = false;

    //}
    //void OnAudioEnd()
    //{
    //    isPlaying = false;

    //    // Este método se llama cuando el sonido ha terminado de reproducirse
    //}
    void FixedUpdate ()
    {

        FaceTime -= Time.deltaTime;

        if(PlayerStatics.PlayerHP <= 0)
        {
            PlayerDies();
            Debug.Log("I DIED");
            StartCoroutine(flashDMG());
            StartCoroutine(disableGetDmg());
        }

        if (gotDamage == true && PlayerStatics.inmuneTimer <= 0 && PlayerStatics.PlayerHP >=1)
        {
            oneDamage();
            StartCoroutine(flashDMG());
            StartCoroutine(disableGetDmg());
            //hpscript.updateHP();
            gotDamage = false;
        }
        if (gotDamage == true && PlayerStatics.inmuneTimer >= 0)
        {
            gotDamage = false;
        }
    }
}
