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

    public List<GameObject> mapSpawns;
    
    public GameObject reviveSpawnArea;
    public GameObject oldArea;
    public GameObject TPspawn;
    public GameObject ReviveTPspawn;
    public float timer;
    public enum spawnOrientation
    {
        R,
        L,
        T,
        B
    }

    private void Start()
    {
        spriteRenderer = blackSquare.GetComponent<SpriteRenderer>();
        virusMove = Player.GetComponent<PlayerMovement>(); ;
        currentColor = Color.clear;
        targetColor = Color.clear;
        fadeSpeed = 6f;
        spriteRenderer.color = currentColor;
        timer = 1f;
        cameraEffect = false;
    }
    public void proceduralMap(spawnOrientation orientation, GameObject newArea = null)
    {
        switch (orientation)
        {
            case spawnOrientation.R:
                if (newArea != null)
                {
                    oldArea.SetActive(false);
                    newArea.SetActive(true);
                    oldArea = newArea;
                }
                else
                {
                    List<GameObject> potentialMaps = new List<GameObject>();
                    foreach (var map in Lvl1maps)
                    {
                        if (!map.name.Contains("R_end"))
                            potentialMaps.Add(map);
                    }

                    GameObject selectedMap = potentialMaps[Random.Range(0, potentialMaps.Count)];

                    mapSpawns.Clear();
                    foreach (Transform child in selectedMap.transform)
                    {
                        if (child.name.Contains("SPAWN"))
                        {
                            mapSpawns.Add(child.gameObject);
                        }
                        if (child.name.Contains("SPAWN_L"))
                        {
                            Transform locationTransform = child.Find("LOCATION");
                            if (locationTransform != null)
                            {
                                TPspawn = locationTransform.gameObject;
                            }
                        }
                    }
                    GameObject oldSpawn = oldArea.transform.Find("SPAWN_R").gameObject;
                    spawnProcedural spawnScript = oldSpawn.GetComponent<spawnProcedural>();
                    spawnScript.mapToSpawn = selectedMap;

                    oldArea.SetActive(false);
                    Instantiate(selectedMap, Vector3.zero, Quaternion.identity);
                    selectedMap.SetActive(true);
                    oldArea = selectedMap;
                }
                break;
            
            case spawnOrientation.L:
                if (newArea != null)
                {
                    oldArea.SetActive(false);
                    newArea.SetActive(true);
                    oldArea = newArea;
                }
                else
                {
                    List<GameObject> potentialMaps = new List<GameObject>();
                    foreach (var map in Lvl1maps)
                    {
                        if (!map.name.Contains("L_end"))
                            potentialMaps.Add(map);
                    }

                    GameObject selectedMap = potentialMaps[Random.Range(0, potentialMaps.Count)];

                    mapSpawns.Clear();
                    foreach (Transform child in selectedMap.transform)
                    {
                        if (child.name.Contains("SPAWN"))
                        {
                            mapSpawns.Add(child.gameObject);
                        }
                        if (child.name.Contains("SPAWN_R"))
                        {
                            Transform locationTransform = child.Find("LOCATION");
                            if (locationTransform != null)
                            {
                                TPspawn = locationTransform.gameObject;
                            }
                        }
                    }
                    GameObject oldSpawn = oldArea.transform.Find("SPAWN_L").gameObject;
                    spawnProcedural spawnScript = oldSpawn.GetComponent<spawnProcedural>();
                    spawnScript.mapToSpawn = selectedMap;

                    oldArea.SetActive(false);
                    Instantiate(selectedMap, Vector3.zero, Quaternion.identity);
                    selectedMap.SetActive(true);
                    oldArea = selectedMap;
                }
                break;
            case spawnOrientation.T:
                if (newArea != null)
                {
                    oldArea.SetActive(false);
                    newArea.SetActive(true);
                    oldArea = newArea;
                }
                else
                {
                    List<GameObject> potentialMaps = new List<GameObject>();
                    foreach (var map in Lvl1maps)
                    {
                        if (!map.name.Contains("T_end")) // Cambiar a "T_end" para la orientación T
                            potentialMaps.Add(map);
                    }

                    GameObject selectedMap = potentialMaps[Random.Range(0, potentialMaps.Count)];

                    mapSpawns.Clear();
                    foreach (Transform child in selectedMap.transform)
                    {
                        if (child.name.Contains("SPAWN"))
                        {
                            mapSpawns.Add(child.gameObject);
                        }
                        if (child.name.Contains("SPAWN_B")) // Cambiar a "SPAWN_B" para la orientación T
                        {
                            Transform locationTransform = child.Find("LOCATION");
                            if (locationTransform != null)
                            {
                                TPspawn = locationTransform.gameObject;
                            }
                        }
                    }
                    GameObject oldSpawn = oldArea.transform.Find("SPAWN_T").gameObject; // Cambiar a "SPAWN_T" para la orientación T
                    spawnProcedural spawnScript = oldSpawn.GetComponent<spawnProcedural>();
                    spawnScript.mapToSpawn = selectedMap;

                    oldArea.SetActive(false);
                    Instantiate(selectedMap, Vector3.zero, Quaternion.identity);
                    selectedMap.SetActive(true);
                    oldArea = selectedMap;
                }
                break;

            case spawnOrientation.B:
                if (newArea != null)
                {
                    oldArea.SetActive(false);
                    newArea.SetActive(true);
                    oldArea = newArea;
                }
                else
                {
                    List<GameObject> potentialMaps = new List<GameObject>();
                    foreach (var map in Lvl1maps)
                    {
                        if (!map.name.Contains("B_end")) // Cambiar a "B_end" para la orientación B
                            potentialMaps.Add(map);
                    }

                    GameObject selectedMap = potentialMaps[Random.Range(0, potentialMaps.Count)];

                    mapSpawns.Clear();
                    foreach (Transform child in selectedMap.transform)
                    {
                        if (child.name.Contains("SPAWN"))
                        {
                            mapSpawns.Add(child.gameObject);
                        }
                        if (child.name.Contains("SPAWN_T")) // Cambiar a "SPAWN_T" para la orientación B
                        {
                            Transform locationTransform = child.Find("LOCATION");
                            if (locationTransform != null)
                            {
                                TPspawn = locationTransform.gameObject;
                            }
                        }
                    }
                    GameObject oldSpawn = oldArea.transform.Find("SPAWN_B").gameObject; // Cambiar a "SPAWN_B" para la orientación B
                    spawnProcedural spawnScript = oldSpawn.GetComponent<spawnProcedural>();
                    spawnScript.mapToSpawn = selectedMap;

                    oldArea.SetActive(false);
                    Instantiate(selectedMap, Vector3.zero, Quaternion.identity);
                    selectedMap.SetActive(true);
                    oldArea = selectedMap;
                }
                break;

        }
    }

    public void spawnAproachBehaviour()
    {
        GameObject closestSpawn = null;
        float minDistance = Mathf.Infinity;

        // Iteramos sobre cada objeto en mapSpawns para calcular su distancia al jugador
        foreach (GameObject spawn in mapSpawns)
        {
            // Calculamos la distancia entre el jugador y el objeto actual en mapSpawns
            float distance = Vector3.Distance(Player.transform.position, spawn.transform.position);

            // Si la distancia es menor que la mínima registrada hasta ahora, actualizamos la mínima y el objeto más cercano
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSpawn = spawn;
            }
        }

        // Si la distancia mínima es menor que 1, el jugador está lo suficientemente cerca de un spawn
        if (closestSpawn != null && minDistance < 1f)
        {
            // Accedemos al script spawnProcedural asociado al spawn más cercano
            spawnProcedural spawnScript = closestSpawn.GetComponent<spawnProcedural>();

            // Si se encuentra el script, ejecutamos proceduralMap con los valores de orientación y mapa
            if (spawnScript != null)
            {
                spawnOrientation orientation = (spawnOrientation)spawnScript.orientation;
                GameObject mapToSpawn = spawnScript.mapToSpawn;
                proceduralMap(orientation, mapToSpawn);
            }
            // Si no se encuentra el script, ejecutamos proceduralMap con la orientación predeterminada
            else
            {
                proceduralMap(spawnOrientation.R);
            }
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
        }
        if (currentColor == Color.black)
        {
            if (oldArea != null)
            {
                oldArea.SetActive(false);

            }
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
        }

        if (currentColor != targetColor)
        {
            currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);
            spriteRenderer.color = currentColor;
        }
    }

    //public void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        if (newArea != null)
    //        {
    //            cameraEffect = true;
    //        }
    //        else
    //        {
    //            //from the list 
    //        }

    //    }
    //}
}
