﻿using System;
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
}
