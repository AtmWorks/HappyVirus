using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    public GameObject blackSquare;

    public float fadeDuration = 2f;

    public GameObject mainCam;
    private SpriteRenderer spriteRenderer;
    private Color targetColor;
    private Color currentColor;
    private float fadeSpeed;

    bool cameraEffect;
    bool blobfix;

    public GameObject Player;   
    public GameObject blobParts;   
    public GameObject newArea;
    public GameObject oldArea;
    public GameObject TPspawn;
    public float timer;
    private void Start()
    {
        spriteRenderer = blackSquare.GetComponent<SpriteRenderer>();
        currentColor = Color.clear;
        targetColor = Color.clear;
        fadeSpeed = 8f;
        spriteRenderer.color = currentColor;
        timer = 1f;
        cameraEffect = false;
        blobfix = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if ( timer >0.2f && targetColor == Color.clear && currentColor != targetColor)
        {
            if (blobfix == true)
            {
                blobParts.SetActive(true);
                blobfix = false;
            }
        }
        if (cameraEffect == true && timer > 1.5)
        {
            if (targetColor == Color.clear)
            {
                targetColor = Color.black;
                timer = 0;
            }
            else
            {
                newArea.SetActive(true);
                
                targetColor = Color.clear;
                cameraEffect = false;
                timer = 0;
            }
        }
        if (currentColor == Color.black )
        {
            oldArea.SetActive(false);
            //Player.SetActive(false);

            Player.transform.position = TPspawn.transform.position;
            mainCam.transform.position = new Vector3( TPspawn.transform.position.x, TPspawn.transform.position.y, mainCam.transform.position.z);
            if (blobParts.activeSelf ==true) { 
                blobfix = true;
                blobParts.SetActive(false); 
            }
            //Player.SetActive(true);

        }

        if (currentColor != targetColor)
        {
            currentColor = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime);
            spriteRenderer.color = currentColor;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cameraEffect = true;

        }
    }

}
