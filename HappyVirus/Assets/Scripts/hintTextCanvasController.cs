using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintTextCanvasController : MonoBehaviour
{
    public UnityEngine.UI.Button okButton;
    public static GameObject childHintText;
    void Start()
    {
        okButton.onClick.AddListener(null);

    }

    void closeChild()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
