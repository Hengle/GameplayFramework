using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorPanel : MonoBehaviour
{
    public Canvas can;
    RectTransform rect;
	// Use this for initialization
	void Start () {
        rect = transform as RectTransform;
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(can.transform as RectTransform, Input.mousePosition, null, out pos))
        {
            rect.localPosition = pos;
        }
    }
}
