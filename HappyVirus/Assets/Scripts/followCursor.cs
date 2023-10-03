using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class followCursor : MonoBehaviour
{
    public GameObject Player2;        
    public float radius;              
    public Vector2 screenPositionMid ;
    public Vector2 mousePositionSprite ;
    public Vector2 mousePositionIngame ;
    public Vector2 mousePositionInCamera;
    public Vector2 mousePositionInScreen;

    private bool insideRange;
    public Vector2 referencePoint;

    //Never mind this if its not a photon project

    private void Start()
    {
        insideRange = true;
        screenPositionMid = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

    }
    public void getCoords() 
    {
        Vector2 ScreenSafe = CursorControl.GetPosition();
        float screenW = Screen.width /2;
        float screenH = Screen.height /2;
        float cursW = CursorControl.GetPosition().x;
        float cursH = CursorControl.GetPosition().y;
        /*
        if (cursH < screenH ) 
        {
            ScreenSafe = new Vector2(ScreenSafe.x, ScreenSafe.y+20);
        }
        if (cursH > screenH ) 
        {
            ScreenSafe = new Vector2(ScreenSafe.x, ScreenSafe.y-20);
        }
        if (cursW < screenW ) 
        {
            ScreenSafe = new Vector2(ScreenSafe.x +20, ScreenSafe.y);
        }
        if (cursW > screenW ) 
        {
            ScreenSafe = new Vector2(ScreenSafe.x - 20, ScreenSafe.y);
        }
        */


        Vector2 ScreenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        referencePoint = ScreenCenter;
    }
    void Update()
    {

        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 playerPos = Player2.transform.position;
        mousePositionIngame = cursorPos;
        mousePositionSprite = this.transform.position;
        Vector2 playerToCursor = cursorPos - playerPos;
        Vector2 dir = playerToCursor.normalized;
        Vector2 cursorVector = dir * radius;
        Vector2 playerRadiusPositive = new Vector2(Player2.transform.position.x + radius, Player2.transform.position.y + radius);
        Vector2 playerRadiusNegative = new Vector2(Player2.transform.position.x - radius, Player2.transform.position.y - radius);
        mousePositionInScreen = CursorControl.GetPosition();

        mousePositionInCamera = Camera.main.ScreenToWorldPoint(cursorPos);


        if (playerToCursor.magnitude < cursorVector.magnitude)
        {
            cursorVector = playerToCursor;

        }// detect if mouse is in inner radius
        if (cursorPos.x > playerRadiusPositive.x || cursorPos.x < playerRadiusNegative.y || cursorPos.y > playerRadiusPositive.y || cursorPos.y < playerRadiusNegative.y)
        {
            if (insideRange == true)
            {
                getCoords();
                insideRange = false;
            }
            

        }
        else { insideRange = true; }

        if (insideRange == true)
        {
            CursorControl.SetPosition(CursorControl.GetPosition());
        }
        else {
            CursorControl.SetPosition(CursorControl.GetPosition());

            //CursorControl.SetPosition(referencePoint);

        }

        //transform.position = playerPos + cursorVector;

        float speed = 0.5f;
        float moveHorizontal = Input.GetAxis("Mouse X");
        float moveVertical = Input.GetAxis("Mouse Y");
        Vector2 spritePos = transform.position;
        Vector2 playerToSprite = spritePos - playerPos;
        Vector2 directions = playerToSprite.normalized;
        Vector2 spriteVector = directions * radius;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        Vector2 getMove = movement * speed;

        if (playerToSprite.magnitude < spriteVector.magnitude)
        {
            spriteVector = playerToCursor;
          
        }
        if (playerToSprite.magnitude > spriteVector.magnitude)
        {
            
            transform.position = playerPos + spriteVector;
        }
        
        transform.position = new Vector2(this.transform.position.x +getMove.x , this.transform.position.y + getMove.y);


    }
}
