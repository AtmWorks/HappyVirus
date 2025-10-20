using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HPbar : MonoBehaviour {

    public Slider healthBarSlider;
    public Text HealthText;
    public int EggCounts;
    public Text O2Text;
    public Text redcurrText;
    public Text bluecurrText;
    public Text EggsText;
    public GameObject[] MaxHPS;
    public GameObject[] CurrentHPS;
    
    public GameObject healEffect;
    public isButtonPressed healButton;
    public bool isHealing;
    public Transform finalTransform;
    public float healTime;


    void Start ()
    {
        healEffect.SetActive(false);
    }

    public void updateHP()
    {

        for (int i = 0; i < CurrentHPS.Length; i++)
        {
            if (i < PlayerCollision.PlayerHP)
            {
                CurrentHPS[i].SetActive(true);
            }
            else
            {
                CurrentHPS[i].SetActive(false);
            }

            if (i < PlayerCollision.PlayermaxHP)
            {
                MaxHPS[i].SetActive(true);
            }
            else
            {
                MaxHPS[i].SetActive(false);
            }
        }
    }

    public void healingProcess()
    {
        if (healButton.buttonPressed)
        {

            if (PlayerCollision.PlayerHP < PlayerCollision.PlayermaxHP && PlayerStatics.O2counter >= 5)
            {

                healEffect.SetActive(true);
                PlayerAnimator.IsInfecting = true;
                healTime += Time.deltaTime;

                if (healTime > 3)
                {
                    PlayerAnimator.IsInfecting = false;
                    healOnce();
                    healTime = 0;
                    healEffect.SetActive(false);
                }
            }

        }
        else if (!healButton.buttonPressed)
        {
            healTime = 0;
            if (healEffect.activeSelf)
            {
                PlayerAnimator.IsInfecting = false;
                healEffect.SetActive(false);
            }
        }
    }
    void Update ()
    {
        EggCounts = PlayerStatics.EggsCounterPubl;
        EggsText.text = "" + EggCounts.ToString() ;
        O2Text.text = "" + PlayerStatics.O2counter.ToString() + "/" + PlayerStatics.maxO2counter ;
        redcurrText.text = "" + PlayerStatics.redCurrCounter.ToString();
        bluecurrText.text = "" + PlayerStatics.blueCurrCounter.ToString();
        updateHP();
        healingProcess();

    }
    public void healOnce()
    {
        PlayerCollision.PlayerHP++;
        PlayerStatics.O2counter -= 5;
        popScore.popText = "-5";
        popScore.isPoping = true;
    }
}
