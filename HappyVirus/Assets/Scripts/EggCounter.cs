using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCounter : MonoBehaviour
{
    public GameObject DobleVirus;
    public GameObject DobleVirus1;

    public bool creationState;
    public bool eggsChosen;
    public int eggsTogether;

    public GameObject trianglePosition;
    public List<GameObject> eggs;
    public List<GameObject> chosenEggs;

    public UnityEngine.UI.Button clonesButton;

    public bool isDemanding;

    public GameObject explosion;
    public void Start()
    {
        isDemanding = false;
        eggsTogether = 0;
        eggsChosen = false;
        clonesButton.onClick.AddListener(changeDemandStart);

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "EGG" && !eggs.Contains(other.gameObject))
        {
            eggs.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "EGG" && eggs.Contains(other.gameObject))
        {
            eggs.Remove(other.gameObject);
        }
    }

    public void changeDemandStart()
    {
        StartCoroutine(changeDemand());
    }
    IEnumerator changeDemand()
    {
        isDemanding = true;
        yield return new WaitForSeconds(0.5f);
        isDemanding = false;
    }

    private void Update()
    {
        //ATRAER HUEVOS AL NEXO DE TRANSFORMACION//
        if ((Input.GetKeyDown("f") || isDemanding )&& PlayerAnimator.IsShooting == false && eggs.Count >= 3)
        {
            creationState = true;
        }
        if (eggs.Count >= 3  && creationState == true)
        {
            Vector3 positionToMeet = trianglePosition.transform.position;
            if (!eggsChosen)
            {
                chosenEggs = new List<GameObject>();
                for (int i = 0; i < 3; i++)
                {
                    if (eggs.Count > 0)
                    {
                        if (!chosenEggs.Contains(eggs[i]))
                        {
                            chosenEggs.Add(eggs[i]);
                        }
                        //eggs.RemoveAt(randomIndex);
                    }
                }

                if(chosenEggs.Count >= 3)
                {
                    foreach (GameObject egg in chosenEggs)
                    {
                        StartCoroutine(MoveEggToPosition(egg, positionToMeet));
                    }
                    eggsChosen = true;

                }


            }

            if (eggsTogether >= 3)
            {
                eggsChosen = false;
                foreach (GameObject egg in chosenEggs)
                {
                    Instantiate(explosion, egg.transform.position, egg.transform.rotation);
                    Destroy(egg);
                    PlayerStatics.EggsCounterPubl--;
                }
                Instantiate(DobleVirus, positionToMeet, DobleVirus.transform.rotation);
                eggs.Clear();
                chosenEggs.Clear();
                creationState = false;
                eggsTogether = 0;
            }
        }


    }

    private IEnumerator MoveEggToPosition(GameObject egg, Vector3 targetPosition)
    {
        float speed = 0.5f; // Ajusta la velocidad de movimiento
        float journeyLength = Vector3.Distance(egg.transform.position, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(egg.transform.position, targetPosition) > 0.5f)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyLength;
            egg.transform.position = Vector3.Lerp(egg.transform.position, targetPosition, fractionOfJourney);
            yield return null;
        }

        eggsTogether++;
    }
}
