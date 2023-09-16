using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D cursor01;
    public GameObject player;
   
    void Start()
    {
        ChangeCursor(cursor01);
        //Cursor.lockState = CursorLockMode.Confined; //el raton se mantiene dentro de la pantalla.
    }
    private void ChangeCursor(Texture2D cursorType)
    {
        Vector2 hotspot = new Vector2(cursorType.width / 2, cursorType.height / 2); //para poner el cursor en el centro y no en la esquina.
        Cursor.SetCursor(cursorType, hotspot, CursorMode.Auto); //sustituir hotspot por Vector2.zero para devolverlo a la esquina.
    }

    //TO DO
    

    void Update()
    {
        
    }
}
