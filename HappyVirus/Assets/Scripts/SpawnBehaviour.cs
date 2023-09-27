using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour
{
    public Animator thisAnimator;

    public GameObject currentArea;
    public GameObject spawnPoint;
    public GameObject virus;
    public PlayerMovement virusMove;

    public FadeToBlack thisTeleport;

    public bool isInfected;

    public spawnController spawnController;

    public List<GameObject> currentTPs;
    

    void Start()
    {
        //TODO: thisAnimator es igual al componente animator de este gameObject 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Virus")
        {

            if(!isInfected)
            {
                //TODO: thisAnimator setBool "isTransforming" true
                thisAnimator.SetBool("isTransforming", true);
                StartCoroutine(infect());
                
            }
            thisTeleport.ReviveTPspawn = spawnPoint;
            spawnController.currentSpawner = this.gameObject;
            spawnController.spawnArea = currentArea;
            UpdateNewTPs();

        }
    }

    public void spawnProcess() {

        //StartCoroutine(animatorBoolWait());
        thisAnimator.SetBool("isSpawning", true);
        StartCoroutine(spawnVirus());



    }
    public void UpdateNewTPs()
    {
        // Asegúrate de que spawnControllerManager y currentTPs no sean nulos.
        if (spawnController != null && currentTPs != null)
        {
            // Vaciar la lista newTPs.
            spawnController.newTPs.Clear();

            // Llenar newTPs con el contenido de currentTPs.
            foreach (GameObject tp in currentTPs)
            {
                spawnController.newTPs.Add(tp);
            }
        }
        else
        {
            Debug.LogWarning("spawnController o currentTPs es null");
        }
    }

    private IEnumerator animatorBoolWait()
    {
        yield return new WaitForSeconds(1.0f); // Espera 1 segundo

        thisAnimator.SetBool("isSpawning", false);
    }
    private IEnumerator spawnVirus()
    {
        yield return new WaitForSeconds(0.5f); // Espera 1 segundo
        //virus.SetActive(true);
        //virus.transform.position = spawnPoint.transform.position;
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

        thisAnimator.SetBool("isInfected", true);
        isInfected = true;
        thisAnimator.SetBool("isTransforming", false);
    }
}
