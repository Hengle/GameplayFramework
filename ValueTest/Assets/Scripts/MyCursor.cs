using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCursor : CursorPanel
{
    /// <summary>
    /// 主鼠标
    /// </summary>
    public GameObject CenterCursor;

    /// <summary>
    /// 锁定图标模板
    /// </summary>
    public GameObject[] LockTargetUITemplate;

    [Tooltip("更新锁定UI的方式，false为Fixed")]
    public bool UpdateInsteadFixedUpdatePawnLock = false;
    public float LockPawnUIUpdateTime = 0.2f;
    /// <summary>
    /// 使用游戏内鼠标
    /// </summary>
    private bool useMycursor;

    public void UseMyCursor()
    {
        useMycursor = true;
        CenterCursor.SetActive(true);
        Cursor.visible = false;
    }

    public void UseSyetemCursor()
    {
        useMycursor = false;
        CenterCursor.SetActive(false);
        Cursor.visible = true;
    }

    /// <summary>
    /// 冷却时间
    /// </summary>
    float cooldownTime4LockPawnUI = 0;
    /// <summary>
    /// UI集合
    /// </summary>
    Dictionary<int, GameObject> lockPawnUIDic = new Dictionary<int, GameObject>();
    /// <summary>
    /// 交换集合
    /// </summary>
    Dictionary<int, GameObject> newDic = new Dictionary<int, GameObject>();
    /// <summary>
    /// UI池
    /// </summary>
    Stack<GameObject> lockPawnUIPool = new Stack<GameObject>();

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        UpdateLockPawnUI(Time.fixedDeltaTime);
    }

    /// <summary>
    /// 更新锁定PawnUI
    /// </summary>
    /// <param name="deltaTime"></param>
    private void UpdateLockPawnUI(float deltaTime)
    {
        if (useMycursor)
        {
            if (cooldownTime4LockPawnUI <= 0)
            {
                foreach (var item in UI.PawnDic)
                {
                    if (!lockPawnUIDic.ContainsKey(item.Key))
                    {
                        lockPawnUIDic[item.Key] = GetLockPawnUI();
                    }

                    UpdateUIPos(item);
                }

                foreach (var item in lockPawnUIDic)
                {
                    item.Value.SetActive(false);
                    lockPawnUIPool.Push(item.Value);
                }

                lockPawnUIDic.Clear();
                lockPawnUIDic.ExchangeTo(newDic);
            }
            else
            {
                cooldownTime4LockPawnUI -= deltaTime;
            }
        }
    }

    private GameObject GetLockPawnUI()
    {
        if (lockPawnUIPool.Count > 0)
        {
            var res = lockPawnUIPool.Pop();
            res.SetActive(true);
            return res;
        }
        else
        {
            var res = GameObject.Instantiate(LockTargetUITemplate[0]);
            return res;
        }
    }

    private void UpdateUIPos(KeyValuePair<int, Poi.Pawn> item)
    {
        ///如果UI已经存在
        var pos = item.Value.transform.position;
        var pos2 = RectTransformUtility.WorldToScreenPoint(null, pos);
        lockPawnUIDic[item.Key].transform.localPosition = pos2;
        newDic[item.Key] = lockPawnUIDic[item.Key];
        lockPawnUIDic.Remove(item.Key);
    }

    protected override void Update()
    {
        base.Update();
        if (UpdateInsteadFixedUpdatePawnLock)
        {
            UpdateLockPawnUI(Time.deltaTime);
        }
    }
}
