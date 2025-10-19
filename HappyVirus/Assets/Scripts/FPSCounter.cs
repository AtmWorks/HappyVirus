using UnityEngine;
using TMPro;
using UnityEngine.Profiling; // para la memoria

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI textoFPS;
    public float updateRate = 4f;

    private float acumulado = 0f;
    private int frames = 0;
    private float tiempoRestante;

    void Awake()
    {
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

            string texto =
                "FPS: " + Mathf.RoundToInt(fpsPromedio) + "\n";
            //"Used Memory: " + (Profiler.GetTotalAllocatedMemoryLong() / 1048576f).ToString("F2") + " MB\n";

#if UNITY_EDITOR
            texto +=
                "Batches: " + UnityEditor.UnityStats.batches + "\n";
                //"SetPass Calls: " + UnityEditor.UnityStats.setPassCalls + "\n" +
                //"Tris: " + UnityEditor.UnityStats.triangles + "\n" +
                //"Verts: " + UnityEditor.UnityStats.vertices + "\n";
#endif

            if (textoFPS != null)
            {
                textoFPS.text = texto;
#if UNITY_ANDROID || UNITY_IOS
                textoFPS.ForceMeshUpdate();
#endif
            }

            tiempoRestante = 1f / updateRate;
            acumulado = 0f;
            frames = 0;
        }
    }
}
