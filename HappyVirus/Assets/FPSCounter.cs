using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [Header("Referencia al texto en pantalla")]
    public TextMeshProUGUI textoFPS;

    [Header("Configuración")]
    public float updateRate = 4f; // veces por segundo que se actualiza

    private float acumulado = 0f;
    private int frames = 0;
    private float tiempoRestante;

    void Awake()
    {
        // Limita los FPS a 60 (puedes subirlo si tu pantalla lo soporta)
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        tiempoRestante = 1f / updateRate;
    }

    void Update()
    {
        float delta = Time.unscaledDeltaTime;
        acumulado += delta;
        frames++;
        tiempoRestante -= delta;

        if (tiempoRestante <= 0f)
        {
            float fpsPromedio = frames / acumulado;
            string texto = "FPS: " + Mathf.RoundToInt(fpsPromedio);

            if (textoFPS != null)
            {
                textoFPS.text = texto;

#if UNITY_ANDROID || UNITY_IOS
                // En móviles a veces no refresca el canvas automáticamente
                textoFPS.ForceMeshUpdate();
#endif
            }

            tiempoRestante = 1f / updateRate;
            acumulado = 0f;
            frames = 0;
        }
    }
}
