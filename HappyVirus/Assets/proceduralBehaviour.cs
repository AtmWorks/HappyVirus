using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static spawnProcedural;

public class proceduralBehaviour : MonoBehaviour
{
    //camera effect related
    public GameObject blackSquare;
    public GameObject mainCam;
    private SpriteRenderer spriteRenderer;
    private Color targetColor;
    private Color currentColor;
    private float fadeSpeed;
    public bool cameraEffect;

    //player related
    public PlayerMovement virusMove;
    public GameObject Player;
    public GameObject blobParts;
    public static List<cloneMovement> clones;
    public bool isReviving;

    //map generation related
    public List<GameObject> Lvl1maps;
    public List<GameObject> mapsSecurityCopy;
    public List<GameObject> usedMaps;

    public spawnProcedural lobbySpawn;

    public GameObject lobbyMap;

    public GameObject oldArea; 
    public GameObject newAreaToSpawn;
    public GameObject TPspawn; 
    public GameObject ReviveTPspawn;
    public float timer;

    private void Start()
    {
        mapsSecurityCopy = new List<GameObject>(Lvl1maps);
        spriteRenderer = blackSquare.GetComponent<SpriteRenderer>();
        virusMove = Player.GetComponent<PlayerMovement>(); ;
        currentColor = Color.clear;
        targetColor = Color.clear;
        fadeSpeed = 6f;
        spriteRenderer.color = currentColor;
        timer = 1f;
        cameraEffect = false;
    }
    public void onRevive()
    {
        Lvl1maps = new List<GameObject>(mapsSecurityCopy);
        foreach (GameObject map in usedMaps) {
        usedMaps.Remove(map);
        Destroy(map);

        }
        lobbySpawn.mapToSpawn = null;

    }
    public void proceduralMap(spawnOrientation orientation, GameObject newArea = null)
    {
        switch (orientation)
        {
            case spawnOrientation.R:
                if (newArea != null)
                {
                    for (int i = 0; i < newArea.transform.childCount; i++)
                    {
                        Transform child = newArea.transform.GetChild(i);
                        if (child.name.Contains("SPAWN_L"))
                        {
                            TPspawn = child.gameObject;
                            spawnProcedural newSpawnScript = child.GetComponent<spawnProcedural>();
                            if (newSpawnScript != null)
                            {
                                newSpawnScript.mapToSpawn = oldArea;
                            }
                            break;
                        }
                        
                    }
                    newAreaToSpawn = newArea;
                    cameraEffect = true;

                }
                else
                {
                    List<GameObject> potentialMaps = new List<GameObject>();
                    foreach (var map in Lvl1maps)
                    {
                        if (!map.name.Contains("R_END") && !map.name.Contains("L_END"))
                            potentialMaps.Add(map);
                    }
                    if (potentialMaps.Count <= 0)
                    {
                        foreach (var map in Lvl1maps)
                        {
                            if (map.name.Contains("L_END"))
                                potentialMaps.Add(map);
                        }
                    }

                    GameObject listSelectedMap = potentialMaps[Random.Range(0, potentialMaps.Count)];
                    Lvl1maps.Remove(listSelectedMap); 
                    GameObject selectedMap = Instantiate(listSelectedMap, Vector3.zero, Quaternion.identity);
                    usedMaps.Add(selectedMap); 
                    GameObject oldSpawn = oldArea.transform.Find("SPAWN_R").gameObject;
                    spawnProcedural oldSpawnScript = oldSpawn.GetComponent<spawnProcedural>();
                    oldSpawnScript.mapToSpawn = selectedMap;

                    foreach (Transform childTransform in selectedMap.transform)
                    {
                        GameObject child = childTransform.gameObject;
                        if (child.name.Contains("SPAWN_L"))
                        {
                            spawnProcedural newSpawnScript = child.GetComponent<spawnProcedural>();
                            if (newSpawnScript != null)
                            {
                                TPspawn = child;
                                newSpawnScript.mapToSpawn = oldArea;
                            }
                            else
                            {
                                Debug.LogWarning("El objeto " + child.name + " no tiene un componente spawnProcedural.");
                            }
                        }
                    }

                    //selectedMap.SetActive(false);
                    newAreaToSpawn = selectedMap;
                    cameraEffect = true;
                }
                break;
                case spawnOrientation.L:
                if (newArea != null)
                {
                    for (int i = 0; i < newArea.transform.childCount; i++)
                    {
                        Transform child = newArea.transform.GetChild(i);
                        if (child.name.Contains("SPAWN_R"))
                        {
                            TPspawn = child.gameObject;
                            spawnProcedural newSpawnScript = child.GetComponent<spawnProcedural>();
                            if (newSpawnScript != null)
                            {
                                newSpawnScript.mapToSpawn = oldArea;
                            }
                            break;
                        }
                        
                    }
                    newAreaToSpawn = newArea;
                    cameraEffect = true;

                }
                else
                {
                    List<GameObject> potentialMaps = new List<GameObject>();
                    foreach (var map in Lvl1maps)
                    {
                        if (!map.name.Contains("R_END") && !map.name.Contains("L_END"))
                            potentialMaps.Add(map);
                    }
                    if (potentialMaps.Count <= 0)
                    {
                        foreach (var map in Lvl1maps)
                        {
                            if (map.name.Contains("R_END"))
                                potentialMaps.Add(map);
                        }
                    }

                    GameObject listSelectedMap = potentialMaps[Random.Range(0, potentialMaps.Count)];
                    Lvl1maps.Remove(listSelectedMap); 
                    GameObject selectedMap = Instantiate(listSelectedMap, Vector3.zero, Quaternion.identity);
                    usedMaps.Add(selectedMap); 
                    GameObject oldSpawn = oldArea.transform.Find("SPAWN_L").gameObject;
                    spawnProcedural oldSpawnScript = oldSpawn.GetComponent<spawnProcedural>();
                    oldSpawnScript.mapToSpawn = selectedMap;

                    foreach (Transform childTransform in selectedMap.transform)
                    {
                        GameObject child = childTransform.gameObject;
                        if (child.name.Contains("SPAWN_R"))
                        {
                            spawnProcedural newSpawnScript = child.GetComponent<spawnProcedural>();
                            if (newSpawnScript != null)
                            {
                                TPspawn = child;
                                newSpawnScript.mapToSpawn = oldArea;
                            }
                            else
                            {
                                Debug.LogWarning("El objeto " + child.name + " no tiene un componente spawnProcedural.");
                            }
                        }
                    }

                    //selectedMap.SetActive(false);
                    newAreaToSpawn = selectedMap;
                    cameraEffect = true;
                }
                break;

            
            

        }
    }

    public void findSoftBodys()
    {
        // Buscar todos los objetos activos en la escena con el tag "subVirus"
        GameObject[] subViruses = GameObject.FindGameObjectsWithTag("SubVirus");

        if (subViruses.Length > 0)
        {
            foreach (GameObject subVirus in subViruses)
            {
                // Verificar si el objeto tiene un componente "cloneMovement"
                cloneMovement cloneMovement = subVirus.GetComponent<cloneMovement>();

                if (cloneMovement != null)
                {
                    // Ejecutar el método "softBodyPosition()" en el componente "cloneMovement"
                    cloneMovement.softBodyPosition();
                }
            }
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.2f && targetColor == Color.clear && currentColor != targetColor)
        {
            virusMove.softBodyPosition();
            findSoftBodys();
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

                if (newAreaToSpawn != null)
                {
                    oldArea.SetActive(false);
                    oldArea = newAreaToSpawn;
                    Player.SetActive(false);
                    newAreaToSpawn.SetActive(true);
                    if (isReviving)
                    {
                        if (ReviveTPspawn != null)
                        {
                            Player.transform.position = ReviveTPspawn.transform.position;
                            mainCam.transform.position = new Vector3(ReviveTPspawn.transform.position.x, ReviveTPspawn.transform.position.y, mainCam.transform.position.z);
                        }

                    }
                    if (!isReviving)
                    {
                        Player.transform.position = TPspawn.transform.position;
                        mainCam.transform.position = new Vector3(TPspawn.transform.position.x, TPspawn.transform.position.y, mainCam.transform.position.z);
                    }
                    Player.SetActive(true);

                }
                targetColor = Color.clear;
                cameraEffect = false;
                timer = 0;
            }
        }

        if (currentColor != targetColor)
        {
            currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);
            spriteRenderer.color = currentColor;
        }
    }

}
