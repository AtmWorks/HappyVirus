using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class changeShootButton : MonoBehaviour
{
    public UnityEngine.UI.Button shootButton;
    public GameObject ImageButton1;
    public GameObject ImageButton2;

    void Start() {
        shootButton.onClick.AddListener(changeShootMode);

    }

    public void changeShootMode()
    {
        if(ParticleFire.shootMode == 1) 
        {
            ParticleFire.shootMode = 3;
            ImageButton1.SetActive(false);
            ImageButton2.SetActive(true);
        }
        else if(ParticleFire.shootMode == 3) 
        {
            ParticleFire.shootMode = 1;
            ImageButton1.SetActive(true);
            ImageButton2.SetActive(false);
        }


    }
}
