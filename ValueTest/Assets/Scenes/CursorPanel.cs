using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorPanel : MonoBehaviour
{
    public float accelerationLimit = 0.75f;
    public float acceleration = 0.13f;

    Canvas canvas;
    RectTransform rect;

    /// <summary>
    /// 使用软件鼠标加速
    /// </summary>
    [Tooltip("使用软件鼠标加速,会引起抖动")]
    public bool useMouseAccelerate = false;

    public bool useFixedUpdate = true;

    // Use this for initialization
    void Start () {
        canvas = GetComponentInParent<Canvas>();
        rect = transform as RectTransform;
        var test = new Vector2(3, 4);
        test.Scale(new Vector2(7, 11));
        Debug.Log(test);
    }
	
	// Update is called once per frame
	void Update ()
    {
        RefreshMyCursorPosition();
    }
    
    readonly Vector2 screen = new Vector2(Screen.width, Screen.height);
    private void RefreshMyCursorPosition()
    {
        Vector2 pos;
        

        if (canvas && RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera, out pos))
        {
            if (useMouseAccelerate)
            {
                Vector2 tempPos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                ///判断连续移动鼠标还是瞬间大距离移动鼠标，如果是瞬间大距离移动，不适用鼠标加速
                bool moveteype = true;

                if (Mathf.Abs(tempPos.x) > accelerationLimit || Mathf.Abs(tempPos.y) > accelerationLimit)
                {
                    moveteype = false;
                }

                if (moveteype)
                {
                    tempPos.Scale(screen);
                    pos += tempPos * acceleration * acceleration;   
                }
            }

            rect.localPosition = pos;
        }
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate)
        {
            RefreshMyCursorPosition();
        }
    }


}
