using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poi;
using UnityEngine;

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
            UseUICursor = !Setting.IsShow;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChatPanel.Show();
        }

        ///显示任何非战斗UI，则使用系统鼠标
        UseUICursor = !(CommandTool.IsShow || Setting.IsShow || HelpMsg.IsShow
            || ChatPanel.IsShow);

        if (Cursor && Cursor.UseMycursor != UseUICursor)
        {
            Cursor.UseMyCursor(UseUICursor);
        } 
    }
}
