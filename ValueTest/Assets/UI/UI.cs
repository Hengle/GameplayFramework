using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poi;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static GM;

public class UI
{
    public static UI Instance { get; private set; } = new UI();

    /// <summary>
    /// 视野中的目标
    /// </summary>
    public static Dictionary<int, IUITarget> PawnDic => Instance.pawnDic;

    public static MyCursor Cursor { get; internal set; }
    public static NameLabel NameLabel { get; internal set; }
    public static HelpMsg HelpMsg { get; internal set; }
    public static Setting Setting { get; internal set; }
    public static bool UseUICursor { get; set; }
    public static bool AutoUI { get; set; } = false;

    public static ChatPanel ChatPanel { get; internal set; }
    

    private UI() { }
    Dictionary<int, IUITarget> pawnDic = new Dictionary<int, IUITarget>();



    internal static void AddPawn(IUITarget pawn)
    {
        int id = pawn.ID;
        if (Instance.pawnDic.ContainsKey(id))
        {
            Instance.pawnDic[id] = pawn;
        }
        else
        {
            Instance.pawnDic.Add(id, pawn);
        }

    }

    internal static void ChangeName(int instanceID, string name, bool useChangeShow)
    {
        NameLabel?.ChangeName(instanceID, name, useChangeShow);
    }

    internal static void RemovePawn(IUITarget pawn)
    {
        int id = pawn.ID;
        Instance.pawnDic.Remove(id);
    }

    internal static List<ISkillTarget> GetLockedTargets()
    {
        return Cursor.GetLockedTargets();
    }

    internal static void CameraLateUpdateCall()
    {
        CameraLateUpdate?.Invoke();
    }

    public static event Action CameraLateUpdate;

   

    public static void Update(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Setting.IsShow = !Setting.IsShow;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //ChatPanel.Show();
        }

        if (AutoUI)
        {
            ///显示任何非战斗UI，则使用系统鼠标
            UseUICursor = !(CommandTool.IsShow || Setting.IsShow || HelpMsg.IsShow
                /*|| ChatPanel.IsShow*/);
        }  

        if (Cursor && Cursor.UseMycursor != UseUICursor)
        {
            Cursor.UseMyCursor(UseUICursor);
        } 
    }

    internal static void Init()
    {
        HelpMsg.Text.text = GM.Instance.HelpMsg;

        List<Dropdown.OptionData> result = new List<Dropdown.OptionData>();
        foreach (var value in ModelDic)
        {
            result.Add(new Dropdown.OptionData(value.Value.Name));
        }

        Setting.selectModel.options = result;
    }
    static HexColor self = new HexColor("45FF0BFF");
    public static string ChatPanel_OnSubmit(string obj)
    {
        GM.SendChat(obj);

        obj = (Player.DataInfo?.Name.SetTMPColor(self) +":") + obj;
        
        return obj;
    }

    static HexColor other = new HexColor("2160DBFF");
    internal static void ShowChatMsg(string name, string context)
    {
        var chat = name.SetTMPColor(other) + ":" + context;
        ChatPanel.ShowChat(chat);
    }
    static HexColor systemMsgColor = new HexColor("FDF600FF");
    internal static void ShowSystemMsg(Poi.CharacterInfo dataInfo)
    {
        string msg = @"玩家" + dataInfo.Name.SetTMPColor(other)+ "已下线";
        msg = msg.SetTMPColor(systemMsgColor);
        ChatPanel.ShowChat(msg,20);
    }
}

public static class TMPStringEX
{
    public static string SetTMPColor(this string input, HexColor color)
    {
        string res = @"<#"+color.ToString()+">" + input + "</color>";
        return res;
    }
}