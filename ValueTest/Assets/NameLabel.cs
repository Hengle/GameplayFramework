using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameLabel : MonoBehaviour
{
    [SerializeField]
    private GameObject template;

    Dictionary<int, Text> UIDic = new Dictionary<int, Text>();
    Dictionary<int, Text> newDic = new Dictionary<int, Text>();
    public Stack<Text> UIPool { get; private set; } = new Stack<Text>();

    Canvas Canvas;
    /// <summary>
    /// 重置名字
    /// </summary>
    private bool isReset;

    private void Awake()
    {
        UI.NameLabel = this;
    }

    private void Start()
    {
        Canvas = this.GetComponentInParent<Canvas>();
    }

    internal void Reset()
    {
        isReset = true;
    }

    // Update is called once per frame
    void Update () {
        foreach (var item in UI.PawnDic)
        {
            var isNew = ConfirmUI(item.Key);
            if (isNew || isReset)
            {
                UIDic[item.Key].text = item.Value.Name;
            }


            Vector2 pos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform,
                item.Value.NamePosition, Canvas.worldCamera, out pos);


            UIDic[item.Key].transform.localPosition = pos;
            newDic[item.Key] = UIDic[item.Key];
            UIDic.Remove(item.Key);
        }

        isReset = false;

        ///清除视野中未锁定的目标
        foreach (var item in UIDic)
        {
            item.Value.gameObject.SetActive(false);
            UIPool.Push(item.Value);
        }
        UIDic.Clear();

        DictionaryExtention.Exchange(ref UIDic, ref newDic);
    }

    /// <summary>
    /// 确认UI存在
    /// </summary>
    /// <param name="id"></param>
    private bool ConfirmUI(int id)
    {
        if (!UIDic.ContainsKey(id))
        {
            Text tempui;
            if (UIPool.Count > 0)
            {
                tempui = UIPool.Pop();
                tempui.gameObject.SetActive(true);
            }
            else
            {
                var tempgo = GameObject.Instantiate(template);
                tempgo.transform.SetParent(transform);
                tempgo.transform.localScale = Vector3.one;
                tempgo.SetActive(true);
                tempui = tempgo.GetComponent<Text>();
            }

            UIDic[id] = tempui;

            return true;
        }

        return false;
    }
}
