using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hintTextController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hintText;
    public GameObject hintBox;
    private Text texto;

    private Color startBoxColor;

    public string hint;
    public bool alreadyRead;

    public bool GameIsPaused;
    public UnityEngine.UI.Button okButton;

    private SpriteRenderer hintBoxRenderer;


    void Start()
    {
       texto = hintText.GetComponent<Text>();
        alreadyRead = false;
        GameIsPaused = false;
        okButton.onClick.AddListener(removeHint);
        hintBoxRenderer = hintBox.GetComponent<SpriteRenderer>();
        startBoxColor = new Color32(0,0,0,156);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            if (!alreadyRead)
            {
                texto.text = hint;

                enableHint();
            }

        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            disableHint();
        }
    }
    
    public void enableHint()
    {
        hintText.SetActive(true);
        //Pause();
        StartCoroutine(FadeInHint());
        alreadyRead = true;
    }
    public void disableHint()
    {
        StartCoroutine(FadeOutHint());
    }

    public void Resume()
    {
        //PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        //PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }


    public void removeHint()
    {
        if (hintText.activeSelf == true)
        {


        }
        //Resume();
    }

  

    private IEnumerator FadeOutHint()
    {
        float duration = 0.5f; // Duración de la transición en segundos.
        float startAlphaText = texto.color.a;
        float startAlphaBox = hintBoxRenderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newAlphaText = Mathf.Lerp(startAlphaText, 0f, elapsedTime / duration);
            float newAlphaBox = Mathf.Lerp(startAlphaBox, 0f, elapsedTime / duration);

            Color newColorText = texto.color;
            newColorText.a = newAlphaText;
            texto.color = newColorText;

            Color newColorBox = hintBoxRenderer.color;
            newColorBox.a = newAlphaBox;
            hintBoxRenderer.color = newColorBox;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que el valor final sea exactamente 0.
        Color finalColorText = texto.color;
        finalColorText.a = 0f;
        texto.color = finalColorText;

        Color finalColorBox = hintBoxRenderer.color;
        finalColorBox.a = 0f;
        hintBoxRenderer.color = finalColorBox;

        // Desactivar el GameObject hintText una vez que la transición haya terminado.
        hintText.SetActive(false);
        Destroy(this.gameObject);
    }

    // Corutina para desvanecer gradualmente la pista.
    // Corutina para desvanecer gradualmente la pista.
    private IEnumerator FadeInHint()
    {
        float duration = 0.5f; // Duración de la transición en segundos.
        float startAlphaText = texto.color.a;
        float startAlphaBox = hintBoxRenderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newAlphaText = Mathf.Lerp(startAlphaText, 1f, elapsedTime / duration);
            float newAlphaBox = Mathf.Lerp(startAlphaBox, 1f, elapsedTime / duration);

            Color newColorText = texto.color;
            newColorText.a = newAlphaText;
            texto.color = newColorText;

            // Cambiar el color de hintBox hacia startBoxColor gradualmente.
            Color newColorBox = Color.Lerp(hintBoxRenderer.color, startBoxColor, elapsedTime / duration); // Cambio aquí
            hintBoxRenderer.color = newColorBox;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que el valor final sea exactamente 1.
        Color finalColorText = texto.color;
        finalColorText.a = 1f;
        texto.color = finalColorText;

        // Asegurarse de que el valor final sea exactamente 1.
        hintBoxRenderer.color = startBoxColor; // Cambio aquí
    }




}
