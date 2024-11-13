using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    //  커서 이미지 파일은 Texture2D
    public Texture2D defaultCursor;
    public Texture2D inGameCursor;

    //  게임 시작시 호출
    private void Start()
    {
        SetDefaultCursor();
    }

    //  인게임에서 커서를 바꿈
    public void SetInGameCursor()
    { 
        Cursor.SetCursor(inGameCursor,Vector2.zero, CursorMode.Auto);
    }

    //  인게임이 아닐 때의 커서 상태
    private void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor,Vector2.zero, CursorMode.Auto);
    }
}
