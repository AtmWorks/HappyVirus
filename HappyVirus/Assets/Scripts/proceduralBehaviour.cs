using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using UnityEditor.Experimental.GraphView;
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
    public GameObject Player;
    public static List<cloneMovement> clones;
    public bool isReviving;

    //map generation related
    public List<GameObject> LVLmaps;
    public List<GameObject> mapsSecurityCopy;
    public List<GameObject> LVLmaps_yellow;

    public bool isEscapeAvailable;
    public int passedRooms;
    public static int difficulty = 0;
    public int currentDifficulty = difficulty;
    public static string lastOrientation;

    //leyenda:
    // 1 2 y 3 (rojo)
    // 4 y 5  (amarillo)


    public List<GameObject> usedMaps;

    public int openPaths = 1;

    public spawnProcedural lobbySpawn;
    public GameObject lobbyMap;

    public GameObject oldArea;
    public GameObject newAreaToSpawn;
    public GameObject TPspawn;
    public GameObject ReviveTPspawn;
    public float timer;


    private void Start()
    {
        mapsSecurityCopy = new List<GameObject>(LVLmaps);
        spriteRenderer = blackSquare.GetComponent<SpriteRenderer>();
        currentColor = Color.clear;
        targetColor = Color.clear;
        fadeSpeed = 6f;
        spriteRenderer.color = currentColor;
        timer = 1f;
        cameraEffect = false;
        openPaths = 1;
        passedRooms = 0;
        isEscapeAvailable = false;
    }
    public void onRevive()
    {
        LVLmaps = new List<GameObject>(mapsSecurityCopy);
        //foreach (GameObject map in usedMaps) {
        //usedMaps.Remove(map);
        //Destroy(map);
        //}
        lobbySpawn.mapToSpawn = null;
        openPaths = 1;
        passedRooms = 0;
    }

    //TODO: si la dificultad supera X, se a�aden las transiciones.
    // si nextLevelType es 0, rojo, 1, amarillo
    // manejar las orientaciones de las transitions (mediante SPAWN)

    public void proceduralMap(spawnOrientation enterSpawnOrientation, int nextLevelType, GameObject areaToSpawn = null)
    {

        string desiredOrientation = "L";
        //consts to switch
        switch (enterSpawnOrientation)
        {
            case spawnOrientation.R:
                desiredOrientation = "L";
                break;
            case spawnOrientation.L:
                desiredOrientation = "R";
                break;
            case spawnOrientation.T:
                desiredOrientation = "B";
                break;
            case spawnOrientation.B:
                desiredOrientation = "T";
                break;
        }
        lastOrientation = desiredOrientation;

        //MAPA YA VISTO
        if (areaToSpawn != null)
        {

            for (int i = 0; i < areaToSpawn.transform.childCount; i++)
            {
                Transform child = areaToSpawn.transform.GetChild(i);
                if (child.gameObject.name.Contains("SPAWN_" + desiredOrientation))
                {
                    TPspawn = child.gameObject;
                    spawnProcedural newSpawnScript = child.gameObject.GetComponent<spawnProcedural>();
                    if (newSpawnScript != null)
                    {
                        newSpawnScript.mapToSpawn = oldArea;
                    }
                    break;
                }

            }
            newAreaToSpawn = areaToSpawn;
            cameraEffect = true;
        }
        //MAPA NUEVO
        else
        {
            passedRooms++;
            openPaths--;
            List<GameObject> listToCheck = new List<GameObject>();
            if (nextLevelType == 0) { 
                Random.InitState(System.DateTime.Now.Millisecond);
                LVLmaps = LVLmaps.OrderBy(x => Random.Range(0, LVLmaps.Count -1)).ToList();
                listToCheck = LVLmaps;
                 }
            else if (nextLevelType == 1) { listToCheck = LVLmaps_yellow; }
            if (listToCheck.Count <= 0)
            {
                onRevive();
                cameraEffect = true;
                return;
            }
            
            if (passedRooms >= 3)
            {
                if (difficulty == 0)
                {
                    isEscapeAvailable = true;
                    difficulty = 1;
                }
            }

            if (passedRooms >= 5)
            {
                if (difficulty == 1)
                {
                    difficulty = 2;
                }
                if (nextLevelType == 1)
                {
                    difficulty = 3;
                }
            }

            //if(passedRooms >= 4) { 
            //    isEscapeAvailable = false;

            //}
            currentDifficulty = difficulty;
            List<GameObject> potentialMaps = new List<GameObject>();
            
            foreach (var map in listToCheck)
            {
                bool shouldBreak = false;
                if (!map.name.Contains(desiredOrientation))
                {
                    shouldBreak = true;
                }
                if (!isEscapeAvailable)
                {
                    if (map.name.Contains("ESC"))
                    {
                        shouldBreak = true;
                    }
                    if (shouldBreak) continue;
                    //si hay mas de 5 caminos abiertos, cerramos seccion
                    if (openPaths >= 5)
                    {
                        if (map.name.Contains(desiredOrientation + "_END"))
                            potentialMaps.Add(map);
                    }
                    //si quedan 2 o menos caminos, no dejamos que haya un cierre de secci�n
                    else if (openPaths <= 2)
                    {
                        if (!map.name.Contains("END"))
                        {
                            potentialMaps.Add(map);
                        }
                    }
                    //si estamos en rango de caminos, da igual que mapa sacar.
                    else
                    {
                        potentialMaps.Add(map);
                    }
                }
                else
                {
                    if (shouldBreak) continue;

                    if (map.name.Contains("ESC"))
                    {
                        potentialMaps.Add(map);

                    }

                }
            }
            if (isEscapeAvailable)
            {
                isEscapeAvailable = false;
                // passedRooms = 0;
            }

            if (potentialMaps.Count <= 0)
            {
                Debug.Log("No hay mapas potenciales disponibles, se añade _END" );
                foreach (var map in usedMaps)
                {
                    if (map.name.Contains(desiredOrientation + "_END"))
                        potentialMaps.Add(map);
                }
            }
            

            GameObject listSelectedMap;

            if (potentialMaps.Count > 0)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                potentialMaps = potentialMaps.OrderBy(x =>Random.Range(0, potentialMaps.Count)).ToList();
                int mapNumber = 0;
                foreach (var map in potentialMaps)
                {
                    mapNumber++;
                    Debug.Log("Map number " + mapNumber + " is " + map.name);
                }
                listSelectedMap = potentialMaps[0];
            }
            else
                listSelectedMap = potentialMaps[0];

            LVLmaps.Remove(listSelectedMap);

            if (listSelectedMap.name.Contains("END"))
            {
                openPaths--;
            }
            else
            {
                for (int i = 0; i < listSelectedMap.transform.childCount; i++)
                {
                    Transform child = listSelectedMap.transform.GetChild(i);
                    if (child.gameObject.name.Contains("SPAWN_"))
                    {
                        openPaths++;
                    }

                }
            }

            Debug.Log("Instantiating this map: " + listSelectedMap.name);
            GameObject selectedMap = Instantiate(listSelectedMap, Vector3.zero, Quaternion.identity);
            spawnProcedural oldSpawnScript = oldArea.transform.Find("SPAWN_" + enterSpawnOrientation).gameObject.GetComponent<spawnProcedural>();
            oldSpawnScript.mapToSpawn = selectedMap;

            foreach (Transform childTransform in selectedMap.transform)
            {
                GameObject child = childTransform.gameObject;
                if (child.name.Contains("SPAWN_" + desiredOrientation))
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
            newAreaToSpawn = selectedMap;
            cameraEffect = true;
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
                    // Ejecutar el m�todo "softBodyPosition()" en el componente "cloneMovement"
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
            Player.GetComponent<SoftBody>().softBodyPosition();
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
