using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIfollow : MonoBehaviour
{
    public GameObject StaminaBar;
    protected Image Bar;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(StaminaBar, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Bar.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Bar.fillAmount = 0.5f;
    }
}
