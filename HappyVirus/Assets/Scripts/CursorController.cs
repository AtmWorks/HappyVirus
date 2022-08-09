using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D cursor01;
    //public Texture2D cursorClick;

    // Start is called before the first frame update
    private void ChangeCursor(Texture2D cursorType)
    {
        Vector2 hotspot = new Vector2(cursorType.width / 2, cursorType.height / 2); //para poner el cursor en el centro y no en la esquina.
        Cursor.SetCursor(cursorType,hotspot,CursorMode.Auto ); //sustituir hotspot por Vector2.zero para devolverlo a la esquina.
    }
    void Start()
    {
        ChangeCursor(cursor01);
        //Cursor.lockState = CursorLockMode.Confined; //el raton se mantiene dentro de la pantalla.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
