using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HPbar : MonoBehaviour {

    public Slider healthBarSlider;
    public int currentHealth;
    public int maxHealth;
    public Text HealthText;
    public int EggCounts;
    public int O2Counter;
    public Text O2Text;
    public Text EggsText;
    bool isDead;
    public GameObject[] MaxHPS;
    public GameObject[] CurrentHPS;
    
    public GameObject healEffect;
    public isButtonPressed healButton;
    public bool isHealButtonPress;
    public bool isHealing;
    public Transform finalTransform;
    public float healTime;


    void Start ()
    {
        isHealButtonPress = false;
        //finalTransform = healEffect.transform;
        //healEffect.transform.localScale = Vector2.zero;
        healEffect.SetActive(false);

    }

    public void updateHP()
    {


        for (int i = 0; i < CurrentHPS.Length; i++)
        {
            if (i < currentHealth)
            {
                CurrentHPS[i].SetActive(true);
            }
            else
            {
                CurrentHPS[i].SetActive(false);
            }

            if (i < maxHealth)
            {
                MaxHPS[i].SetActive(true);
            }
            else
            {
                MaxHPS[i].SetActive(false);
            }
        }

        if (currentHealth < 0 || currentHealth > CurrentHPS.Length - 1 || maxHealth < 0 || maxHealth > MaxHPS.Length - 1)
        {
            //Debug.Log("health exception");
        }
    }
    void Update ()
    {
        EggCounts = PlayerStatics.EggsCounterPubl;
        O2Counter = PlayerStatics.O2counter;
        maxHealth = PlayerCollision.PlayermaxHP;
        currentHealth = PlayerCollision.PlayerHP;
        EggsText.text = "" + EggCounts.ToString() ;
        O2Text.text = "" + O2Counter.ToString() + "/" + PlayerStatics.maxO2counter ;
        updateHP();
        isHealButtonPress = healButton.buttonPressed;
        if (isHealButtonPress)
        {
            Debug.Log("BUTTON HEAL IS PRESSED");
            if(PlayerCollision.PlayerHP < PlayerCollision.PlayermaxHP && O2Counter >= 5) 
            {
                Debug.Log("STARTING TO HEAL");
                healEffect.SetActive(true);
                PlayerAnimator.IsInfecting= true;
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
        else if (!isHealButtonPress) {
            healTime = 0;

            if(healEffect.activeSelf)
            {
                PlayerAnimator.IsInfecting = false;
                healEffect.SetActive(false);

            }
        }

    }
    public void healOnce()
    {
        Debug.Log("HEALED ONE");

        PlayerCollision.PlayerHP++;
        PlayerStatics.O2counter -= 5;
        popScore.popText = "-5";
        popScore.isPoping = true;
    }
}
