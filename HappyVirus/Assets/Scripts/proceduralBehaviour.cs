using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<GameObject> LVLmaps;
    public List<GameObject> mapsSecurityCopy;
    public bool isEscapeAvailable;
    public int passedRooms;
    public static int difficutly = 0; 

    //new try map generation
    //Nombres de los mapas: 
    /*
     * HORIZONTAL
     * VERTICAL
     * QUAD
     * TRIPLE_R
     * TRIPLE_L
     * TRIPLE_T
     * TRIPLE_B
     * END_R
     * END_L
     * END_T
     * END_B
    */



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
        virusMove = Player.GetComponent<PlayerMovement>(); ;
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

    //public void proceduralMap(spawnOrientation enterSpawnOrientation, int nextLevelType , GameObject areaToSpawn = null )
    //{
    //    string desiredOrientation = "L";
    //    List<string> mapBans = new List<string>();
    //    //consts to switch
    //    switch (enterSpawnOrientation)
    //    {
    //        case spawnOrientation.R:
    //            desiredOrientation = "L";
    //            mapBans = new List<string>
    //            {
    //            "VERTICAL",
    //            "TRIPLE_L",
    //            "R_END",
    //            "T_END",
    //            "B_END"
    //            };
    //        break;
    //        case spawnOrientation.L:
    //            desiredOrientation = "R";
    //            mapBans = new List<string>
    //            {
    //            "VERTICAL",
    //            "TRIPLE_R",
    //            "L_END",
    //            "T_END",
    //            "B_END"
    //            };
    //        break;
    //        case spawnOrientation.T:
    //            desiredOrientation = "B";
    //            mapBans = new List<string>
    //            {
    //            "HORIZONTAL",
    //            "TRIPLE_B",
    //            "R_END",
    //            "L_END",
    //            "T_END"
    //            };
    //        break;
    //        case spawnOrientation.B:
    //            desiredOrientation = "T";
    //            mapBans = new List<string>
    //            {
    //            "HORIZONTAL",
    //            "TRIPLE_T",
    //            "R_END",
    //            "L_END",
    //            "B_END"
    //            };
    //        break;
            
            
    //    }
    //    if ( areaToSpawn != null )
    //    {

    //        for (int i = 0; i < areaToSpawn.transform.childCount; i++)
    //        {
    //            Transform child = areaToSpawn.transform.GetChild(i);
    //            if (child.gameObject.name.Contains("SPAWN_"+desiredOrientation))
    //            {
    //                Debug.Log("Encontrado:"+ child.gameObject.name);
    //                //ponemos el punto de teleport en el spawn de salida
    //                TPspawn = child.gameObject;
    //                //linkamos el mapa del que venimos a ese spawn de salida
    //                spawnProcedural newSpawnScript = child.gameObject.GetComponent<spawnProcedural>();
    //                if (newSpawnScript != null)
    //                {
    //                    newSpawnScript.mapToSpawn = oldArea;
    //                }
    //                break;
    //            }
                
    //        }
    //        newAreaToSpawn = areaToSpawn;
    //        cameraEffect = true;
    //    }
    //    else
    //    {
    //        //Si nos quedamos sin mapas, de momento para el lobby.
    //        if (LVLmaps.Count <= 0)
    //        {
    //            onRevive();
    //            cameraEffect = true;
    //            return;
    //        }
    //        //Se cierra un path
    //        openPaths--;
    //        //Mapas potenciales
    //        Debug.Log("FROM:" + enterSpawnOrientation + " TO " + desiredOrientation);
    //        List<GameObject> potentialMaps = new List<GameObject>();
    //        Debug.Log("ENTERING MAP BANS");
    //        int mapFetching = 0;
    //        foreach (var map in LVLmaps)
    //        {
    //            mapFetching++;
    //            Debug.Log("Fetching map:" + mapFetching+" "+ map.name);
    //            bool shouldBreak = false;

    //            foreach (string mapBan in mapBans)
    //            {
    //                Debug.Log($"{mapBan}");
    //                if (map.name.Contains(mapBan))
    //                {
    //                    Debug.Log("Map banned:" + map.name);
    //                    shouldBreak = true;
    //                    break; // Cambiado a continue
    //                }
    //            }
    //            if (shouldBreak) continue;
    //            //si hay mas de 5 caminos abiertos, cerramos seccion
    //            if (openPaths >= 5)
    //            {
    //                if (map.name.Contains(desiredOrientation+"_END"))
    //                    potentialMaps.Add(map);
    //            }
    //            //si quedan 2 o menos caminos, no dejamos que haya un cierre de sección
    //            else if (openPaths <= 2)
    //            {
    //                if (!map.name.Contains("END"))
    //                {
    //                    potentialMaps.Add(map);
    //                }
    //            }
    //            //si estamos en rango de caminos, da igual que mapa sacar.
    //            else
    //            {
    //                potentialMaps.Add(map);
    //            }
    //        }
    //        string potentialMapsString = "Potential maps after filter: ";
    //        foreach (var map in potentialMaps)
    //        {
    //            potentialMapsString += map.name + ", ";
    //        }
    //        Debug.Log(potentialMapsString);

    //        if (potentialMaps.Count <= 0)
    //        {
    //            foreach (var map in usedMaps)
    //            {
    //                if (map.name.Contains(desiredOrientation+"_END"))
    //                    potentialMaps.Add(map);
    //            }
    //        }
    //        //Debug.Log("Didnt get potential maps, adding used map:" + potentialMaps[0]);
    //        //seleccionamos un mapa aleatorio dentro de posibles mapas.
    //        if (potentialMaps.Count > 0)
    //        {
    //            Debug.Log("Numero de mapas potenciales"+potentialMaps.Count);
    //        }
    //        else
    //        {
    //            Debug.Log("No hay mapas potenciales disponibles, se añade _END" + potentialMaps.Count);
    //            foreach (var map in LVLmaps )
    //            {
    //                Debug.Log("Emergencia: " + map.name);

    //                if (map.name.Contains(desiredOrientation + "_END"))
    //                    potentialMaps.Add(map);
    //            }
    //        }

    //        GameObject listSelectedMap;
    //        if (potentialMaps.Count > 0)
    //        listSelectedMap = potentialMaps[Random.Range(0, potentialMaps.Count)];
    //        else
    //        listSelectedMap = potentialMaps[0];

    //        LVLmaps.Remove(listSelectedMap);

    //        if (listSelectedMap.name.Contains("QUAD"))
    //        {
    //            openPaths += 3;
    //        }
    //        else if (listSelectedMap.name.Contains("TRIPLE"))
    //        {
    //            openPaths += 2;
    //        }
    //        else if (listSelectedMap.name.Contains("HORIZONTAL") || listSelectedMap.name.Contains("VERTICAL"))
    //        {
    //            openPaths++;
    //        }
    //        else if (listSelectedMap.name.Contains("END"))
    //        {
    //            openPaths--;
    //        }
    //        Debug.Log("Instantiating this map: " + listSelectedMap.name);
    //        GameObject selectedMap = Instantiate(listSelectedMap, Vector3.zero, Quaternion.identity);
    //        spawnProcedural oldSpawnScript = oldArea.transform.Find("SPAWN_"+enterSpawnOrientation).gameObject.GetComponent<spawnProcedural>();
    //        oldSpawnScript.mapToSpawn = selectedMap;

    //        foreach (Transform childTransform in selectedMap.transform)
    //        {
    //            GameObject child = childTransform.gameObject;
    //            if (child.name.Contains("SPAWN_"+desiredOrientation))
    //            {
    //                spawnProcedural newSpawnScript = child.GetComponent<spawnProcedural>();
    //                if (newSpawnScript != null)
    //                {
    //                    TPspawn = child;
    //                    newSpawnScript.mapToSpawn = oldArea;
    //                }
    //                else
    //                {
    //                    Debug.LogWarning("El objeto " + child.name + " no tiene un componente spawnProcedural.");
    //                }
    //            }
    //        }
    //        newAreaToSpawn = selectedMap;
    //        cameraEffect = true;
    //    }
    //}
    public void proceduralMap(spawnOrientation enterSpawnOrientation, int nextLevelType , GameObject areaToSpawn = null )
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
        if ( areaToSpawn != null )
        {

            for (int i = 0; i < areaToSpawn.transform.childCount; i++)
            {
                Transform child = areaToSpawn.transform.GetChild(i);
                if (child.gameObject.name.Contains("SPAWN_"+desiredOrientation))
                {
                    Debug.Log("Encontrado:"+ child.gameObject.name);
                    //ponemos el punto de teleport en el spawn de salida
                    TPspawn = child.gameObject;
                    //linkamos el mapa del que venimos a ese spawn de salida
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
        else
        {
            if (LVLmaps.Count <= 0)
            {
                onRevive();
                cameraEffect = true;
                return;
            }
            passedRooms++;
            openPaths--;
            if(passedRooms >= 4) { 
                isEscapeAvailable = true;
                if (difficutly == 0)
                {
                    difficutly = 1;
                }
            }
            List<GameObject> potentialMaps = new List<GameObject>();
            Debug.Log("ENTERING MAP BANS");
            int mapFetching = 0;
            foreach (var map in LVLmaps)
            {
                mapFetching++;
                Debug.Log("Fetching map:" + mapFetching+" "+ map.name);
                bool shouldBreak = false;

                if (!map.name.Contains(desiredOrientation))
                {
                    Debug.Log("Map banned:" + map.name);
                    shouldBreak = true; 
                }
                if(!isEscapeAvailable)
                {
                    if (map.name.Contains("ESC"))
                    {
                        Debug.Log("Map banned:" + map.name);
                        shouldBreak = true;
                    }
                    if (shouldBreak) continue;
                    //si hay mas de 5 caminos abiertos, cerramos seccion
                    if (openPaths >= 5)
                    {
                        if (map.name.Contains(desiredOrientation + "_END"))
                            potentialMaps.Add(map);
                    }
                    //si quedan 2 o menos caminos, no dejamos que haya un cierre de sección
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
                passedRooms = 0;
            }

            if (potentialMaps.Count <= 0)
            {
                foreach (var map in usedMaps)
                {
                    if (map.name.Contains(desiredOrientation+"_END"))
                        potentialMaps.Add(map);
                }
            }
            //Debug.Log("Didnt get potential maps, adding used map:" + potentialMaps[0]);
            //seleccionamos un mapa aleatorio dentro de posibles mapas.
            if (potentialMaps.Count > 0)
            {
                Debug.Log("Numero de mapas potenciales"+potentialMaps.Count);
            }
            else
            {
                Debug.Log("No hay mapas potenciales disponibles, se añade _END" + potentialMaps.Count);
                foreach (var map in LVLmaps )
                {
                    Debug.Log("Emergencia: " + map.name);

                    if (map.name.Contains(desiredOrientation + "_END"))
                        potentialMaps.Add(map);
                }
            }

            GameObject listSelectedMap;
            if (potentialMaps.Count > 0)
            listSelectedMap = potentialMaps[Random.Range(0, potentialMaps.Count)];
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
            spawnProcedural oldSpawnScript = oldArea.transform.Find("SPAWN_"+enterSpawnOrientation).gameObject.GetComponent<spawnProcedural>();
            oldSpawnScript.mapToSpawn = selectedMap;

            foreach (Transform childTransform in selectedMap.transform)
            {
                GameObject child = childTransform.gameObject;
                if (child.name.Contains("SPAWN_"+desiredOrientation))
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
