using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spawnController : MonoBehaviour
{

    public GameObject currentSpawner;
    public SpawnBehaviour spawnBehaviour;
    public GameObject currentArea;
    public GameObject spawnArea;
    public proceduralBehaviour proceduralBehaviour;
    //public GameObject TPspawn;

    //public List<GameObject> currentTPlist;
    //public List<GameObject> newTPs;
    // Start is called before the first frame update
    void Start()
    {
        //TODO: spawnBehaviour es un script que esta como componente del gameObject currentSpawner
        spawnBehaviour = currentSpawner.GetComponent<SpawnBehaviour>();
    }

    public void spawnProcess()
    {
        if (currentSpawner != null)
        {
            //TODO: RESETEAR VALORES DEL NIVEL

            proceduralBehaviour.oldArea.SetActive(false);
            proceduralBehaviour.oldArea = spawnArea;
            proceduralBehaviour.onRevive();
            spawnArea.SetActive(true);
            currentSpawner.SetActive(true);
            spawnBehaviour = currentSpawner.GetComponent<SpawnBehaviour>();
            
            spawnBehaviour.spawnProcess();
            //StartCoroutine(gestionTPs());
            

        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        
    }
    //IEnumerator gestionTPs()
    //{
    //    // Espera 1 segundo.
    //    yield return new WaitForSeconds(0.5f);

    //    //// Activa los objetos en la lista newTPs.
    //    //foreach (GameObject obj in newTPs)
    //    //{
    //    //    obj.SetActive(true);
    //    //}

    //    //// Desactiva los objetos en la lista oldTPs.
    //    //foreach (GameObject obj in currentTPlist)
    //    //{
    //    //    obj.SetActive(false);
    //    //}

    //    // Desactiva el objeto que porta este script.
    //    //gameObject.SetActive(false);
    //}
}
