using System;
using System.Collections;
using System.Collections.Generic;
using Poi;
using UnityEngine;

public class MyCursor : MonoBehaviour
{
    
    /// <summary>
    /// 主鼠标
    /// </summary>
    public CenterCursor CenterCursor;

    /// <summary>
    /// 锁定图标模板
    /// </summary>
    public GameObject[] LockTargetUITemplate;

    public float LockDistance { get; set; } = 100;

    /// <summary>
    /// 使用游戏内鼠标
    /// </summary>
    private bool useMycursor;

    public void UseMyCursor()
    {
        useMycursor = true;
        CenterCursor.gameObject.SetActive(true);
        Cursor.visible = false;
    }

    public void UseSyetemCursor()
    {
        useMycursor = false;
        CenterCursor.gameObject.SetActive(false);
        Cursor.visible = true;
    }

    

    Canvas Canvas;

    private void Awake()
    {
        UI.Cursor = this;
    }

    private void Start()
    {
        Canvas = this.GetComponentInParent<Canvas>();
    }



    /// <summary>
    /// 冷却时间
    /// </summary>
    float cooldownTime4LockPawnUI = 0;
    /// <summary>
    /// UI集合
    /// </summary>
    Dictionary<int, LockTargetUI> lockPawnUIDic = new Dictionary<int, LockTargetUI>();
    /// <summary>
    /// 交换集合
    /// </summary>
    Dictionary<int, LockTargetUI> newDic = new Dictionary<int, LockTargetUI>();
    /// <summary>
    /// UI池
    /// </summary>
    Stack<LockTargetUI> lockPawnUIPool = new Stack<LockTargetUI>();
    private List<ISkillTarget> skillTargetList = new List<ISkillTarget>();

    public Camera UICamera => Camera.main;

    void FixedUpdate()
    {
        UpdateCenterUI();

        UpdateLockPawnUI(Time.fixedDeltaTime);
    }

    private void UpdateCenterUI()
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform,
            Input.mousePosition, Canvas.worldCamera, out pos))
        {
            CenterCursor.transform.localPosition = pos;
        }
    }

    public List<ISkillTarget> GetLockedTargets()
    {
        return skillTargetList;
    }

    /// <summary>
    /// 更新锁定PawnUI
    /// </summary>
    /// <param name="deltaTime"></param>
    private void UpdateLockPawnUI(float deltaTime)
    {
        if (useMycursor)
        {
            var tempskillTargetList = new List<ISkillTarget>();
            foreach (var item in UI.PawnDic)
            {
                ConfirmUI(item.Key);

                ///从视野中的目标中选出被游标锁定的目标
                if (item.Value.magnitudeToMouse < LockDistance)
                {
                    //Vector3 temppos =  Camera.main.WorldToScreenPoint(item.Value.transform.position);

                    Vector2 pos;

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform,
                        item.Value.ScreenPosition, Canvas.worldCamera, out pos);
                        

                    lockPawnUIDic[item.Key].transform.localPosition = pos;
                    newDic[item.Key] = lockPawnUIDic[item.Key];
                    lockPawnUIDic.Remove(item.Key);


                    ///加入技能目标集合
                    tempskillTargetList.Add(item.Value as ISkillTarget);
                }

            }

            ///清除视野中未锁定的目标
            foreach (var item in lockPawnUIDic)
            {
                item.Value.gameObject.SetActive(false);
                lockPawnUIPool.Push(item.Value);
            }
            lockPawnUIDic.Clear();

            DictionaryExtention.Exchange(ref lockPawnUIDic,ref newDic);            
        }
    }

    /// <summary>
    /// 确认UI存在
    /// </summary>
    /// <param name="id"></param>
    private void ConfirmUI(int id)
    {
        if (!lockPawnUIDic.ContainsKey(id))
        {
            lockPawnUIDic[id] = GetLockPawnUI();
        }
    }

    /// <summary>
    /// 取得一个或者创建一个新的锁定UI
    /// </summary>
    /// <returns></returns>
    private LockTargetUI GetLockPawnUI()
    {
        if (lockPawnUIPool.Count > 0)
        {
            var res = lockPawnUIPool.Pop();
            res.gameObject.SetActive(true);
            res.ReActive = true;
            return res;
        }
        else
        {
            var lockedUI = GameObject.Instantiate(LockTargetUITemplate[0]);
            lockedUI.transform.SetParent(transform);
            lockedUI.transform.localScale = Vector3.one;
            lockedUI.SetActive(true);
            var res = lockedUI.GetComponent<LockTargetUI>();
            res.ReActive = true;
            return res;
        }
    }

    void Update()
    {
        UpdateCenterUI();
        UpdateLockPawnUI(Time.deltaTime);
    }
}
