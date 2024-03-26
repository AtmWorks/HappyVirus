using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintTextCanvasController : MonoBehaviour
{
    public UnityEngine.UI.Button yesButton;
    public UnityEngine.UI.Button noButton;
    public static GameObject childHintText;
    public PlayerCollision playerScript;
    public proceduralBehaviour proceduralMap;


    void Start()
    {
        yesButton.onClick.AddListener(backToBase);
        noButton.onClick.AddListener(backToNormal);
        proceduralMap = GameObject.Find("Procedural map").GetComponent<proceduralBehaviour>();

        if (proceduralMap == null)
        {
            Debug.LogError("Procedural map object not found in the scene.");
        }
    }

    void backToNormal()
    {
       Time.timeScale = 1f;
       this.gameObject.SetActive(false);
    }
    private void backToBase()
    {
        Time.timeScale = 1f;
        playerScript.PlayerDies();
        proceduralMap.onRevive();
        this.gameObject.SetActive(false);
    }

}
