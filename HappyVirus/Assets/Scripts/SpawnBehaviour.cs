using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour
{
    public GameObject Player;
    public Animator thisAnimator;
    public GameObject currentArea;
    public GameObject spawnPoint;
    public bool isInfected;
    public spawnController spawnController = null;
    public bool isMainSpawner;
    public GameObject leaveUI;
    public bool askOnce;
    //public List<GameObject> currentTPs;
    

    void Start()
    {
        //if Player is not setted, find it in scene, its a gameObject with the name "Virus"
        if (!Player)
        {
            Player = GameObject.Find("Virus");
        }

        askOnce = false;
        GameObject leaveUIParent = GameObject.Find("leaveUI");
        if (leaveUIParent != null)
        {
            if (leaveUIParent.transform.childCount > 0)
            {
                leaveUI = leaveUIParent.transform.GetChild(0).gameObject;
            }
            else
            {
                Debug.LogError("El objeto leaveUI no tiene hijos.");
            }
        }
        else Debug.LogError("No se encontró el objeto leaveUI en la escena.");

        // Busca el objeto hijo dentro de leaveUIParent
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Virus")
        {

            if(!isInfected)
            {
                thisAnimator.SetBool("isTransforming", true);
                StartCoroutine(infect());
            }
            if(isMainSpawner)
            {
                spawnController.currentSpawner = this.gameObject;
                spawnController.spawnArea = currentArea;
            }
        }
    }

    public void spawnProcess() {

        //StartCoroutine(animatorBoolWait());
        thisAnimator.SetBool("isSpawning", true);
        // Player.SetActive(true);
        Player.transform.position = spawnPoint.transform.position;
        Player.GetComponent<SoftBody>().softBodyPosition();
        StartCoroutine(spawnVirus());
    }
    private IEnumerator animatorBoolWait()
    {
        yield return new WaitForSeconds(1.0f); // Espera 1 segundo

        thisAnimator.SetBool("isSpawning", false);
    }
    private IEnumerator spawnVirus()
    {
        yield return new WaitForSeconds(0.5f); // Espera 1 segundo
        
        thisAnimator.SetBool("isSpawning", false);
    }
    void Update()
    {
        
        if (isInfected) {
            thisAnimator.SetBool("isInfected", true);
        }
    }

    IEnumerator infect()
    {
        // Espera 1 segundo.
        yield return new WaitForSeconds(1f);
        //if (!isMainSpawner)
        //{
        //    if (!askOnce)
        //    {
        //        yield return StartCoroutine(askForBack());
        //    }
        //}
        // Si es el spawner principal o si askOnce ya es verdadero, 
        // establecer isInfected y la animación de transformación
        thisAnimator.SetBool("isInfected", true);
        isInfected = true;
        thisAnimator.SetBool("isTransforming", false);
    }
    IEnumerator askForBack()
    {
        //esto no se esta ejecutando
        if (!askOnce)
        {
            askOnce = true;
            yield return new WaitForSeconds(3f);
            //Esto si se está ejecutando
            leaveUI.SetActive(true);
            Time.timeScale = 0f;

        }


    }
}
