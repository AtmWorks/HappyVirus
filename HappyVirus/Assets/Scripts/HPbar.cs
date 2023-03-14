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
        Debug.Log("CURRENT HP LENGHT IS " + CurrentHPS.Length);
        Debug.Log("MAX LENGHT IS " + MaxHPS.Length);

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
            Debug.Log("health exception");
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
