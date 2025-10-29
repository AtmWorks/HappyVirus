using UnityEngine;

public class CameraFadeEffect : MonoBehaviour
{
    public SpriteRenderer fadeSpriteRenderer;

    public GameObject mainCam;
    private Color targetColor;
    private Color currentColor;
    public float fadeDuration = 1.5f;
    private float fadeSpeed;
    public bool blackFadeEffect;
    public bool cameraShakeEffect;

    public float shakeDuration = 0.5f;

    public float shakeAmount = 1f;
    public float timer = 1f;
    private void Start()
    {
        currentColor = Color.clear;
        targetColor = Color.clear;
        fadeSpeed = 8f;
        fadeSpriteRenderer.color = currentColor;
        blackFadeEffect = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        // if ( timer >0.2f && targetColor == Color.clear && currentColor != targetColor)
        // {
        //     //logica antigua de softbodyposition
        // }
        if (blackFadeEffect == true)
        {
            manageBlackFadeEffect(fadeDuration > 0 ? fadeDuration : 1.5f);
        }
        
        manageCameraShakeEffect(shakeDuration, new Vector3(shakeAmount, shakeAmount, 0));

        //Always on display
        colorTransition();
    }

    public void manageBlackFadeEffect(float duration)
    {
        if (timer > duration){
            // En el siguiente if se controla el cambio de color, si es negro a transparente o viceversa.
            if (targetColor == Color.clear)
            {
                targetColor = Color.black;
                timer = 0;
            }
            else
            {
                targetColor = Color.clear;
                blackFadeEffect = false;
                timer = 0;
            }
            }
    }

    public void manageCameraShakeEffect(float duration, Vector3 amount)
    {
        if (cameraShakeEffect)
        {
            //Cosas a tener en cuenta: se debe tener en cuenta el ''timer'' para saber cuando dejar de hacer shake
            //Se controlará el tiempo actual y se le sumará duration una vez para saber cuando dejar de hacer shake
            //Como esta funcion estará en el Update, deberemos evitar de algun modo que se sume la duracion cada vez que se ejecute esta funcion
            //Haremos un shake de la camara desde hasta que ''timer'' llegue al punto de ''timer + duration'' que se debió establecer una sola vez
            //Al llegar a ese punto, se desactivará el efecto de shake
            //El shake tendrá en cuenta ''amount'' para saber la intensidad del shake
        }
    }

    public void colorTransition()
    {
        if (currentColor != targetColor)
        {
            currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);
            fadeSpriteRenderer.color = currentColor;
        }
    }

    

}
