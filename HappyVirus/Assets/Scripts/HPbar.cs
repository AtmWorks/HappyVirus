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
    


	void Start ()
    {

        
        //maxHealth = PlayerCollision.PlayermaxHP;
        
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
        O2Text.text = "O²:" + O2Counter.ToString() ;
    }
}
