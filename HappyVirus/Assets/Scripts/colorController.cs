using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class colorController : MonoBehaviour
{
    public int colorState;
    public List<Color> colorList;
    public SpriteRenderer thisSpriteRenderer = null;
    private SpriteShapeRenderer thisShapeRenderer = null;
    private void Start()
    {
        // Intenta obtener el componente SpriteRenderer
        thisSpriteRenderer = GetComponent<SpriteRenderer>();

        // Si no se encontró SpriteRenderer, intenta obtener SpriteShapeRenderer
        if (thisSpriteRenderer == null)
        {
            thisShapeRenderer = GetComponent<SpriteShapeRenderer>();
        }
    }
    void Update()
    {
        if (PlayerStatics.colorState != colorState)
        {
            colorState = PlayerStatics.colorState;
        }
        if (thisShapeRenderer != null && thisShapeRenderer.color != colorList[colorState])
        {
            thisShapeRenderer.color = colorList[colorState];
        }
        if (thisSpriteRenderer != null && thisSpriteRenderer.color != colorList[colorState])
        {
            thisSpriteRenderer.color = colorList[colorState];
        }
    }
}
