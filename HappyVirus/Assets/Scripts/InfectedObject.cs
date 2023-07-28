using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedObject : MonoBehaviour
{

    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public SpriteRenderer sprite3;
    public float colorTransitionSpeed = 1.0f;
    public string virusTag = "Virus";

    private bool isInfected = false;
    private Color originalColor1;
    private Color originalColor2;
    private Color originalColor3;
    private Color infectedColor = new Color(0x2B / 255f, 1.0f, 0x00 / 255f);

    void Start()
    {
        originalColor1 = sprite1.color;
        originalColor2 = sprite2.color;
        originalColor3 = sprite3.color;
    }

    void Update()
    {
        if (isInfected)
        {
            // Do nothing if already infected
            return;
        }

        // Check if the collider is in contact with the virus
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        bool contactWithVirus = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == virusTag)
            {
                contactWithVirus = true;
                break;
            }
        }

        // Gradually change the color of the sprites
        if (contactWithVirus)
        {
            ChangeColorGradually(sprite1, infectedColor);
            ChangeColorGradually(sprite2, infectedColor);
            ChangeColorGradually(sprite3, infectedColor);
        }
        else
        {
            ChangeColorGradually(sprite1, originalColor1);
            ChangeColorGradually(sprite2, originalColor2);
            ChangeColorGradually(sprite3, originalColor3);
        }
    }

    void ChangeColorGradually(SpriteRenderer sprite, Color targetColor)
    {
        Color currentColor = sprite.color;

        // If the current color is already the target color, return
        if (currentColor == targetColor)
        {
            return;
        }

        // Gradually change the color towards the target color
        Color newColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorTransitionSpeed);
        sprite.color = newColor;

        // Check if the new color is close enough to the target color
        if (ColorDistance(newColor, targetColor) < 0.01f)
        {
            sprite.color = targetColor;

            // If the color has changed to the infected color, set the isInfected flag to true
            if (targetColor == infectedColor)
            {
                isInfected = true;
            }
        }
    }

    float ColorDistance(Color c1, Color c2)
    {
        return Mathf.Sqrt(Mathf.Pow(c1.r - c2.r, 2) + Mathf.Pow(c1.g - c2.g, 2) + Mathf.Pow(c1.b - c2.b, 2));
    }
}
