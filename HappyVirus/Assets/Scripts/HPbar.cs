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


	void Start ()
    {

        
        //maxHealth = PlayerCollision.PlayermaxHP;
        
	}

    public void updateHP()
    {
        /*
        //Activa las que sean antes de currentHealth
        for (int i = 0; i > currentHealth - 1; i++)
        {
            CurrentHPS[i].SetActive(true);
        }
        //Desactiva las que pasen currentHealth hasta maxHealth
        for (int i = currentHealth; i > maxHealth - 1; i++)
        {
            CurrentHPS[i].SetActive(false);
        }
        //Activa y desactiva los max health
        for (int i = 0; i > maxHealth - 1; i++)
        {
            MaxHPS[i].SetActive(true);
        }
        for (int i = MaxHPS.Length; i < maxHealth - 1; i--)
        {
            MaxHPS[i].SetActive(false);
        }*/
        /*
        if (currentHealth >= 6)
        {
            CurrentHPS[0].SetActive(true);
            CurrentHPS[1].SetActive(true);
            CurrentHPS[2].SetActive(true);
            CurrentHPS[3].SetActive(true);
            CurrentHPS[4].SetActive(true);
            CurrentHPS[5].SetActive(true);
        }
        else if(currentHealth==5)
        {

        }*/

        Debug.Log("CURRENT HP LENGHT IS " + CurrentHPS.Length);
        Debug.Log("MAX LENGHT IS " + MaxHPS.Length);
        switch (currentHealth)
        {
            case 6:
                //desactiva
                CurrentHPS[0].SetActive(true);
                CurrentHPS[1].SetActive(true);
                CurrentHPS[2].SetActive(true);
                CurrentHPS[3].SetActive(true);
                CurrentHPS[4].SetActive(true);
                CurrentHPS[5].SetActive(true);
                break;

                
            case 5:
                CurrentHPS[0].SetActive(true);
                CurrentHPS[1].SetActive(true);
                CurrentHPS[2].SetActive(true);
                CurrentHPS[3].SetActive(true);
                CurrentHPS[4].SetActive(true);
                CurrentHPS[5].SetActive(false);
                break;
            case 4:
                CurrentHPS[0].SetActive(true);
                CurrentHPS[1].SetActive(true);
                CurrentHPS[2].SetActive(true);
                CurrentHPS[3].SetActive(true);
                CurrentHPS[4].SetActive(false);
                CurrentHPS[5].SetActive(false);
                break;
            case 3:
                CurrentHPS[0].SetActive(true);
                CurrentHPS[1].SetActive(true);
                CurrentHPS[2].SetActive(true);
                CurrentHPS[3].SetActive(false);
                CurrentHPS[4].SetActive(false);
                CurrentHPS[5].SetActive(false);
                break;
            case 2:
                CurrentHPS[0].SetActive(true);
                CurrentHPS[1].SetActive(true);
                CurrentHPS[2].SetActive(false);
                CurrentHPS[3].SetActive(false);
                CurrentHPS[4].SetActive(false);
                CurrentHPS[5].SetActive(false);
                break;
            case 1:
                CurrentHPS[0].SetActive(true);
                CurrentHPS[1].SetActive(false);
                CurrentHPS[2].SetActive(false);
                CurrentHPS[3].SetActive(false);
                CurrentHPS[4].SetActive(false);
                CurrentHPS[5].SetActive(false);
                break;
            case 0:
                CurrentHPS[0].SetActive(false);
                CurrentHPS[1].SetActive(false);
                CurrentHPS[2].SetActive(false);
                CurrentHPS[3].SetActive(false);
                CurrentHPS[4].SetActive(false);
                CurrentHPS[5].SetActive(false);
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }

        switch (maxHealth)
        {
            case 6:
                //desactiva
                MaxHPS[0].SetActive(true);
                MaxHPS[1].SetActive(true);
                MaxHPS[2].SetActive(true);
                MaxHPS[3].SetActive(true);
                MaxHPS[4].SetActive(true);
                MaxHPS[5].SetActive(true);
                break;
            case 5:
                MaxHPS[0].SetActive(true);
                MaxHPS[1].SetActive(true);
                MaxHPS[2].SetActive(true);
                MaxHPS[3].SetActive(true);
                MaxHPS[4].SetActive(true);
                MaxHPS[5].SetActive(false);
                break;
            case 4:
                MaxHPS[0].SetActive(true);
                MaxHPS[1].SetActive(true);
                MaxHPS[2].SetActive(true);
                MaxHPS[3].SetActive(true);
                MaxHPS[4].SetActive(false);
                MaxHPS[5].SetActive(false);
                break;
            case 3:
                MaxHPS[0].SetActive(true);
                MaxHPS[1].SetActive(true);
                MaxHPS[2].SetActive(true);
                MaxHPS[3].SetActive(false);
                MaxHPS[4].SetActive(false);
                MaxHPS[5].SetActive(false);
                break;
            case 2:
                MaxHPS[0].SetActive(true);
                MaxHPS[1].SetActive(true);
                MaxHPS[2].SetActive(false);
                MaxHPS[3].SetActive(false);
                MaxHPS[4].SetActive(false);
                MaxHPS[5].SetActive(false);
                break;
            case 1:
                MaxHPS[0].SetActive(true);
                MaxHPS[1].SetActive(false);
                MaxHPS[2].SetActive(false);
                MaxHPS[3].SetActive(false);
                MaxHPS[4].SetActive(false);
                MaxHPS[5].SetActive(false);
                break;
            case 0:
                MaxHPS[0].SetActive(false);
                MaxHPS[1].SetActive(false);
                MaxHPS[2].SetActive(false);
                MaxHPS[3].SetActive(false);
                MaxHPS[4].SetActive(false);
                MaxHPS[5].SetActive(false);
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }



    }
    void Update ()
    {
        EggCounts = PlayerStatics.EggsCounterPubl;
        O2Counter = PlayerStatics.O2counter;
        maxHealth = PlayerCollision.PlayermaxHP;
        currentHealth = PlayerCollision.PlayerHP;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
        HealthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        EggsText.text = "Eggs:" + EggCounts.ToString() ;
        O2Text.text = "" + O2Counter.ToString() ;
        updateHP();










    }
}
