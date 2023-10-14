using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    public GameObject blackSquare;

    public float fadeDuration = 2f;
    
    public spawnController spawnController;

    public GameObject mainCam;
    private SpriteRenderer spriteRenderer;
    private Color targetColor;
    private Color currentColor;
    private float fadeSpeed;

    public PlayerMovement virusMove;


    public bool cameraEffect;
    //bool blobfix;

    public GameObject Player;   
    public GameObject blobParts;   
    public GameObject newArea;
    public GameObject oldArea;
    public GameObject TPspawn;
    public GameObject ReviveTPspawn;

    public List<GameObject> newTPs;
    public List<GameObject> oldTPs;

    public ResetObjectsToDefault resetComponent;


    public bool isReviving;

    public float timer;
    private void Start()
    {
        spriteRenderer = blackSquare.GetComponent<SpriteRenderer>();
        virusMove = Player.GetComponent<PlayerMovement>(); ;

        currentColor = Color.clear;
        targetColor = Color.clear;
        fadeSpeed = 8f;
        spriteRenderer.color = currentColor;
        timer = 1f;
        cameraEffect = false;
        //blobfix = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if ( timer >0.2f && targetColor == Color.clear && currentColor != targetColor)
        {
            virusMove.softBodyPosition();
            //if (blobfix == true)
            //{
            //    blobParts.SetActive(true);
            //    blobfix = false;
            //}
        }
        if (cameraEffect == true && timer > 1.5)
        {
            if (targetColor == Color.clear)
            {
                targetColor = Color.black;
                timer = 0;
            }
            else
            {
                if(newArea != null)
                {
                    newArea.SetActive(true);
                    // Obtén una referencia al componente ResetObjectsToDefault
                    
                    ResetObjectsToDefault resetComponent = newArea.GetComponent<ResetObjectsToDefault>();

                    // Verifica si el componente se encontró antes de llamar al método
                    if (resetComponent != null)
                    {
                        // Llama al método ResetToDefault() en el componente ResetObjectsToDefault
                        resetComponent.ResetToDefault();
                    }
                    else
                    {
                        Debug.LogError("El componente ResetObjectsToDefault no se encuentra en el objeto newArea.");
                    }

                }

                targetColor = Color.clear;
                cameraEffect = false;
                timer = 0;

                StartCoroutine(gestionTPs());

            }
        }
        if (currentColor == Color.black )
        {
            if (oldArea != null)
            {
                oldArea.SetActive(false);

            }
            //Player.SetActive(false);
            if (isReviving)
            {
                if (ReviveTPspawn!=null) 
                {
                    //StartCoroutine(dameUnRespiro());
                    Player.transform.position = ReviveTPspawn.transform.position;
                    mainCam.transform.position = new Vector3(ReviveTPspawn.transform.position.x, ReviveTPspawn.transform.position.y, mainCam.transform.position.z);
                }
                
            }
            if (!isReviving)
            {
                Player.transform.position = TPspawn.transform.position;
                mainCam.transform.position = new Vector3(TPspawn.transform.position.x, TPspawn.transform.position.y, mainCam.transform.position.z);
            }


            

            //if (blobParts.activeSelf ==true) { 
            //    blobfix = true;
            //    blobParts.SetActive(false); 
            //}

            //Player.SetActive(true);

        }

        if (currentColor != targetColor)
        {
            currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);
            spriteRenderer.color = currentColor;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            spawnController.currentArea = newArea;
            cameraEffect = true;

        }
    }

    IEnumerator gestionTPs()
    {
        // Espera 1 segundo.


        // Desactiva los objetos en la lista oldTPs.
        foreach (GameObject obj in oldTPs)
        {
            obj.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        // Activa los objetos en la lista newTPs.
        foreach (GameObject obj in newTPs)
        {
            obj.SetActive(true);
        }



        UpdateNewTPs();

        // Desactiva el objeto que porta este script.
        if (!isReviving) gameObject.SetActive(false);


    } 
    IEnumerator dameUnRespiro()
    {
        // Espera 1 segundo.
        yield return new WaitForSeconds(0.2f);
        spawnController.spawnProcess();

    }

    public void UpdateNewTPs()
    {
        // Asegúrate de que spawnControllerManager y currentTPs no sean nulos.
        if (spawnController != null && newTPs != null)
        {
            // Vaciar la lista newTPs.
            spawnController.currentTPlist.Clear();

            // Llenar newTPs con el contenido de currentTPs.
            foreach (GameObject tp in newTPs)
            {
                spawnController.currentTPlist.Add(tp);
            }
        }
        else
        {
            Debug.LogWarning("spawnController o currentTPs es null");
        }
    }

}
