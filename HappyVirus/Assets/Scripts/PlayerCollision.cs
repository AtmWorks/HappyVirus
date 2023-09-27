using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour {

    public GameObject virusParent;
    public GameObject explosion;
    public GameObject spawn;
    public HPbar hpscript;
    public static int PlayermaxHP;
    public static int PlayerHP ;
    public static float InmuneTime ;
    public static float FaceTime ;
    public static bool gotDamage;
    public spawnController spawnController;
    public PlayerMovement player;

    public FadeToBlack thisTeleport;


    [SerializeField]private List <SimpleFlash> flashList;

    //PARA EFECTO DE DAÑO



    // public GameObject PlayerObject;

    void Start ()
    {
        PlayermaxHP = 3;
        PlayerHP = 3;
        gotDamage = false;
    }

    void PlayerDies ()
    {
        //this.gameObject.SetActive(false);
        //SPAWN:
        //virusParent.transform.position = spawn.transform.position;
        //this.transform.parent.transform.position = spawn.transform.position;
        //PlayerHP = 3;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        thisTeleport.isReviving = true;
        thisTeleport.cameraEffect = true;
        StartCoroutine(dameUnRespiro());



    }
    IEnumerator dameUnRespiro()
    {
        // Espera 1 segundo.
        yield return new WaitForSeconds(1f);
        PlayerHP = 3;
        player.blobCircle.fillAmount = 0;
        
        player.blobCircle.color = new Color32(95, 255, 100, 255);

        spawnController.spawnProcess();

    }
    void oneDamage()
    {
        PlayerHP -= 1;
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
        Debug.Log("ENTRO EN EL METODO FLASH");
        foreach (SimpleFlash flash in flashList)
        {
            Debug.Log("ENTRO EN EL BUCLE FLASH");
            flash.Flash();

        }
        yield return null;
    }
    private void OnCollisionStay2D(Collision2D PlayerCol )
    {
        if (PlayerCol.gameObject.tag == "Damage" && PlayerStatics.inmuneTimer <= 0 && FaceTime <= 0)
        {
            
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

    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spawn")
        {
            spawn = collision.gameObject;
            Debug.Log("IM ON SPAWN");

        }
    }


    void FixedUpdate ()
    {

        FaceTime -= 1 * Time.deltaTime;
        if (FaceTime <= 0)
        {
            PlayerAnimator.GetDmg = false;
        }

        if(PlayerHP < 0)
        { 
            PlayerDies();
            Debug.Log("I DIED");
        }

        if (gotDamage == true && PlayerStatics.inmuneTimer <= 0)
        {

            

            PlayerAnimator.GetDmg = true;
            PlayerStatics.inmuneTimer = 1f;
            FaceTime = 1f;
            oneDamage();
            StartCoroutine(flashDMG());
            //hpscript.updateHP();
            gotDamage = false;



        }

    }
}
