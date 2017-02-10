using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorPanel : MonoBehaviour
{
    Canvas canvas;
    RectTransform rect;
	// Use this for initialization
	void Start () {
        canvas = GetComponentInParent<Canvas>();
        rect = transform as RectTransform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        RefreshMyCursorPosition();
    }

    private void RefreshMyCursorPosition()
    {
        Vector2 pos;
        if (canvas && RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera, out pos))
        {
            rect.localPosition = pos;
        }
    }

    private void FixedUpdate()
    {
        RefreshMyCursorPosition();
    }


}
