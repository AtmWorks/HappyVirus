using System.Collections;
using System.Collections.Generic;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class spikerEnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp;
    public List<SimpleFlash> FlashList;
    public spikerEnemyBehaviour parent;
   

    void Start()
    {
        if (hp==0) hp = 8;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Proyectil")
        {
            if (hp > 0)
            {
                foreach (SimpleFlash flash in FlashList)
                {
                    flash.Flash();
                }
                hp--;
            }

        }
        

    }
    private void Update()
    {
       if (hp <= 0 )
       {
            parent.isDead = true;

        }
       
    }

    
}
