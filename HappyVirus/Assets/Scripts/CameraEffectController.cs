using System.Collections;
using UnityEngine;

public class CameraEffectController : MonoBehaviour
{
    [Header("Fade Settings")]
    public SpriteRenderer fadeSpriteRenderer;
    public float defaultFadeTime = 0.5f;
    public float defaultFadeDuration = 1.5f;

    [Header("Shake Settings")]
    public GameObject mainCam;
    public float defaultShakeDuration = 0.3f;
    public float defaultShakeAmount = 0.1f;

    // --- Estado interno ---
    private Color _originalFadeColor = Color.clear;
    private Vector3 _originalCamLocalPos;
    private Coroutine _fadeCo;
    private Coroutine _shakeCo;

    private void Start()
    {
        if (fadeSpriteRenderer != null)
        {
            _originalFadeColor = Color.clear;
            fadeSpriteRenderer.color = _originalFadeColor;
        }

        if (mainCam == null && Camera.main != null)
            mainCam = Camera.main.gameObject;

        if (mainCam != null)
            _originalCamLocalPos = mainCam.transform.localPosition;
    }

    // ------------------ Triggers públicos ------------------

    /// <summary>
    /// Inicia un efecto de fade. Si ya hay uno activo, lo ignora.
    /// fadeTime: tiempo de transición (a negro o desde negro)
    /// fadeDuration: tiempo en negro
    /// </summary>
    public void triggerFade(float fadeTime = 0.5f, float fadeDuration = 1.5f)
    {
        if (_fadeCo == null)
            _fadeCo = StartCoroutine(FadeRoutine(fadeTime, fadeDuration));
    }

    /// <summary>
    /// Inicia un efecto de shake. Si ya hay uno activo, lo ignora.
    /// duration: cuánto dura el shake.
    /// amount: intensidad del movimiento (opcional).
    /// </summary>
    public void triggerShake(float duration = 0.3f, float amount = 0.1f)
    {
        if (_shakeCo == null)
            _shakeCo = StartCoroutine(ShakeRoutine(duration, new Vector3(amount, amount, 0f)));
    }

    // ------------------ Corrutinas ------------------

    /// <summary>
    /// Fade con tres fases:
    /// - Clear → Black (fadeTime)
    /// - Mantener negro (fadeDuration)
    /// - Black → Clear (fadeTime)
    /// </summary>
    private IEnumerator FadeRoutine(float fadeTime, float fadeDuration)
    {
        if (fadeSpriteRenderer == null)
        {
            _fadeCo = null;
            yield break;
        }

        // Fase 1: claro → negro
        yield return LerpColor(fadeSpriteRenderer, Color.clear, Color.black, Mathf.Max(0.0001f, fadeTime));

        // Fase 2: mantener negro
        yield return new WaitForSeconds(Mathf.Max(0.0001f, fadeDuration));

        // Fase 3: negro → claro
        yield return LerpColor(fadeSpriteRenderer, Color.black, Color.clear, Mathf.Max(0.0001f, fadeTime));

        fadeSpriteRenderer.color = Color.clear;
        _fadeCo = null;
    }

    /// <summary>
    /// Sacudida de cámara durante 'duration' con intensidad 'amount' (X,Y).
    /// </summary>
    private IEnumerator ShakeRoutine(float duration, Vector3 amount)
    {
        if (mainCam == null)
        {
            _shakeCo = null;
            yield break;
        }

        _originalCamLocalPos = mainCam.transform.localPosition;

        float endTime = Time.time + Mathf.Max(0.0001f, duration);
        while (Time.time < endTime)
        {
            float offsetX = Random.Range(-amount.x, amount.x);
            float offsetY = Random.Range(-amount.y, amount.y);
            Vector3 randomOffset = new Vector3(offsetX, offsetY, 0f);

            mainCam.transform.localPosition = _originalCamLocalPos + randomOffset;
            yield return null;
        }

        mainCam.transform.localPosition = _originalCamLocalPos;
        _shakeCo = null;
    }

    // ------------------ Utilidades ------------------

    private static IEnumerator LerpColor(SpriteRenderer sr, Color from, Color to, float duration)
    {
        float t = 0f;
        sr.color = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / duration);
            sr.color = Color.LerpUnclamped(from, to, alpha);
            yield return null;
        }

        sr.color = to;
    }

    public void CancelAllEffectsAndRestore()
    {
        if (_fadeCo != null) { StopCoroutine(_fadeCo); _fadeCo = null; }
        if (_shakeCo != null) { StopCoroutine(_shakeCo); _shakeCo = null; }

        if (fadeSpriteRenderer != null) fadeSpriteRenderer.color = Color.clear;
        if (mainCam != null) mainCam.transform.localPosition = _originalCamLocalPos;
    }
}
