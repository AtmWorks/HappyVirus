using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;
using UnityEngine.U2D;

public class yellowBossBehaviour : MonoBehaviour
{
    public List<GameObject> spawners;
    private float timer;
    public Animator animator;
    public GameObject enemyPrefab;
    public GameObject plaquetaPrefab;
    public cloneMovementBoss movement;
    public List<SimpleFlash> flashes;
    public int bossHP;
    private bool didSpawn;
    public GameObject parent;
    public Transform parentInitialtransform;
    public GameObject blob1;
    public GameObject blob2;
    public GameObject blob3;
    public GameObject blob4;
    public GameObject blob5;
    public GameObject blob6;

    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;


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

    public GameObject BossSkin;
    public GameObject BossBody;

    public GameObject upgradeObj;

    public List <SpriteRenderer> bossSpriteRenderers;
    public SpriteShapeRenderer skinRender;

    void Start()
    {
        didSpawn=false; 
        timer = -12f; // Se establece el temporizador en un valor aleatorio entre -6 y -3.
        //parentInitialtransform = parent.transform;


        softPos1 = blob1.transform.localPosition;
        softPos2 = blob2.transform.localPosition;
        softPos3 = blob3.transform.localPosition;
        softPos4 = blob4.transform.localPosition;
        softPos5 = blob5.transform.localPosition;
        softPos6 = blob6.transform.localPosition;
        softRot = blob1.transform.rotation;

        skinPos = BossSkin.transform.localPosition;
        skinRot = BossSkin.transform.localRotation;
        skinScale = BossSkin.transform.localScale;
    }

    public void softBodyPosition()
    {
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
        BossSkin.gameObject.transform.localPosition = skinPos;
        BossSkin.gameObject.transform.localRotation = skinRot;
        BossSkin.gameObject.transform.localScale = skinScale;

    }

    void Update()
    {
        timer += Time.deltaTime;
        bossHpController();
        spawningProcess();
    }

    void spawningProcess() 
    {
        if (timer > -2 && timer < 1 && didSpawn == false)
        {
            //parent.transform.localScale = Vector3.Lerp(parent.transform.localScale, parentInitialtransform.localScale * 1.005f, Time.deltaTime);
            animator.SetBool("isSpawning", true);
            movement.enabled = false;
            Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
        }
        if (timer > 0 && didSpawn == false)
        {
            //parent.transform.localScale = parentInitialtransform.localScale;
            Spawnvfx();
            animator.SetBool("isSpawning", false);
            didSpawn = true;
        }
        if (timer > 0.2 && timer < 0.8)
        {
            softBodyPosition();
        }
        if (timer > 2)
        {
            timer = Random.Range(-10f, -8f);
            movement.enabled = true;
            didSpawn = false;

        }
    }
    public void bossHpController()
    {

//TODO: crea una variables delos siguientes color: R=85 G=255 B=75 A=200
        if (bossHP <= 25 && bossHP > 15)
        {
            //TODO:los sprite renderers de la Lista de renderers publica declarada anteriormente pasaran su color actual al color1 de manera gradual y suave en un plazo de 1 segundo
            foreach (SpriteRenderer renderer in bossSpriteRenderers)
            {
                renderer.color = Color.Lerp(renderer.color, color1, Time.deltaTime);
            }
            skinRender.color = Color.Lerp(skinRender.color, color1, Time.deltaTime);
        }
        if (bossHP <= 15 && bossHP > 10)
        {
            foreach (SpriteRenderer renderer in bossSpriteRenderers)
            {
                renderer.color = Color.Lerp(renderer.color, color2, Time.deltaTime);
            }
            skinRender.color = Color.Lerp(skinRender.color, color2, Time.deltaTime);

        }
        if (bossHP < 10 && bossHP >= 5)
        {
            foreach (SpriteRenderer renderer in bossSpriteRenderers)
            {
                renderer.color = Color.Lerp(renderer.color, color3, Time.deltaTime);
            }
            skinRender.color = Color.Lerp(skinRender.color, color3, Time.deltaTime);
        }
        if (bossHP < 5 && bossHP >= 0)
        {
            foreach (SpriteRenderer renderer in bossSpriteRenderers)
            {
                renderer.color = Color.Lerp(renderer.color, color4, Time.deltaTime);
            }
            skinRender.color = Color.Lerp(skinRender.color, color4, Time.deltaTime);
        }

        if (bossHP <= 0)
        {
            Instantiate(upgradeObj, parent.transform.position, parent.transform.rotation);
            Destroy(parent);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Proyectil")
        {
            bossHP--;
            foreach (SimpleFlash flash in flashes)
            {
                flash.Flash();
            }
        }
    }
    void Spawnvfx()
    {
        
        foreach (GameObject spawner in spawners)
        {
            int randomSpawn = Random.Range(1, 4);
            if (randomSpawn ==1)
            {
                Instantiate(enemyPrefab, spawner.transform.position, spawner.transform.rotation);
            }
            else if (randomSpawn >= 2 && randomSpawn < 4)
            {
                Instantiate(plaquetaPrefab, spawner.transform.position, spawner.transform.rotation);
            }
            else if (randomSpawn == 4)
            {
                Instantiate(plaquetaPrefab, spawner.transform.position, spawner.transform.rotation);
                Instantiate(plaquetaPrefab, spawner.transform.position, spawner.transform.rotation);
                Instantiate(plaquetaPrefab, spawner.transform.position, spawner.transform.rotation);
            }
            // Instantiate un enemigo en la posición del spawner.
        }
    }
}
