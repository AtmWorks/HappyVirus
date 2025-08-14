using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI textoFPS;  // Asignar en el inspector
    public float updateRate = 4f;     // Veces por segundo que se actualiza el valor

    private float acumulado = 0f;
    private int frames = 0;
    private float tiempoRestante;

    void Start()
    {
        tiempoRestante = 1f / updateRate;
    }

    void Update()
    {
        float delta = Time.unscaledDeltaTime; // No afectado por Time.timeScale
        acumulado += 1f / delta;
        frames++;
        tiempoRestante -= delta;

        if (tiempoRestante <= 0f)
        {
            float fpsPromedio = acumulado / frames;
            textoFPS.text = "FPS: " + Mathf.RoundToInt(fpsPromedio);

            tiempoRestante = 1f / updateRate;
            acumulado = 0f;
            frames = 0;
        }
    }
}
