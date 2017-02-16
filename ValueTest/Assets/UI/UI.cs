using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poi;

public class UI
{
    public static UI Instance { get; private set; } = new UI();

    public static Dictionary<int, Pawn> PawnDic => Instance.pawnDic;

    private UI() { }
    Dictionary<int, Pawn> pawnDic = new Dictionary<int, Pawn>();
    internal static void AddPawn(Pawn pawn)
    {
        int id = pawn.DataInfo.ID;
        if (Instance.pawnDic.ContainsKey(id))
        {
            Instance.pawnDic[id] = pawn;
        }
        else
        {
            Instance.pawnDic.Add(id, pawn);
        }

    }

    internal static void RemovePawn(Pawn pawn)
    {
        int id = pawn.DataInfo.ID;
        Instance.pawnDic.Remove(id);
    }
}
