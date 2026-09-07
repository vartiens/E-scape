using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private bool cursorVisible = true;

    void Start()
    {
        ShowCursor();
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        cursorVisible = false;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        cursorVisible = true;
    }

    public bool IsCursorVisible()
    {
        return cursorVisible;
    }
}
