using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D inGameCursor;

    private void Start()
    {
        SetDefaultCursor();
    }

    public void SetInGameCursor()
    { 
        Cursor.SetCursor(inGameCursor,Vector2.zero, CursorMode.Auto);
    }

    private void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor,Vector2.zero, CursorMode.Auto);
    }
}
