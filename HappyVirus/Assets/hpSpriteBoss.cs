using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpSpriteBoss : MonoBehaviour
{
    public GameObject hp1;
    public GameObject hp2;
    public GameObject hp3;
    public GameObject hp4;
    public GameObject hp5;
    public GameObject hp6;
    public bossDMG bosshp;

    // Start is called before the first frame update
    void Start()
    {
        hp1.SetActive(true);
        hp2.SetActive(false);
        hp3.SetActive(false);
        hp4.SetActive(false);
        hp5.SetActive(false);
        hp6.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bosshp.bossHP >= 50) {
            hp1.SetActive(true);
            hp2.SetActive(false);
            hp3.SetActive(false);
            hp4.SetActive(false);
            hp5.SetActive(false);
            hp6.SetActive(false);
        }
        if (bosshp.bossHP >= 40 && bosshp.bossHP <= 49) {
            hp1.SetActive(false);
            hp2.SetActive(true);
            hp3.SetActive(false);
            hp4.SetActive(false);
            hp5.SetActive(false);
            hp6.SetActive(false);
        }
        else if (bosshp.bossHP >= 30 && bosshp.bossHP<=39) {
            hp1.SetActive(false);
            hp2.SetActive(false);
            hp3.SetActive(true);
            hp4.SetActive(false);
            hp5.SetActive(false);
            hp6.SetActive(false);
        }
        else if (bosshp.bossHP >= 20 && bosshp.bossHP<=29) {
            hp1.SetActive(false);
            hp2.SetActive(false);
            hp3.SetActive(false);
            hp4.SetActive(true);
            hp5.SetActive(false);
            hp6.SetActive(false);
        }
        else if (bosshp.bossHP >= 10 && bosshp.bossHP<=19) {
            hp1.SetActive(false);
            hp2.SetActive(false);
            hp3.SetActive(false);
            hp4.SetActive(false);
            hp5.SetActive(true);
            hp6.SetActive(false);
        }
        else if (bosshp.bossHP >= 0 && bosshp.bossHP<=19) {
            hp1.SetActive(true);
            hp2.SetActive(false);
            hp3.SetActive(false);
            hp4.SetActive(false);
            hp5.SetActive(false);
            hp6.SetActive(true);
        }

    }
}
