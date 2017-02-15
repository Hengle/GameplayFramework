using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCursor : CursorPanel
{
    public GameObject CenterCursor;
    // Use this for initialization
    public GameObject[] LockTargetUIModel;

    public void UseMyCursor()
    {
        CenterCursor.SetActive(true);
        Cursor.visible = false;
    }

    public void UseSyetemCursor()
    {
        CenterCursor.SetActive(false);
        Cursor.visible = true;
    }
}
