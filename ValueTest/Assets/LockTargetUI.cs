using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTargetUI : MonoBehaviour
{

    public Vector2 NextLocalPosition { get; internal set; }
    /// <summary>
    /// 重新激活 瞬移到目标位置，否则插值到目标位置
    /// </summary>
    public bool ReActive { get; internal set; }

    RectTransform rectTrans;

    private float moveTime = 0.2f;

    // Use this for initialization
    void Start () {
        rectTrans = transform as RectTransform;
	}
	
	// Update is called once per frame
	void Update () {
        if (ReActive)
        {
            rectTrans.localPosition = NextLocalPosition;
            ReActive = false;
        }
        else
        {
            rectTrans.localPosition = 
                Vector2.LerpUnclamped(rectTrans.localPosition, NextLocalPosition, Time.deltaTime/moveTime);   
        }
    }
}
