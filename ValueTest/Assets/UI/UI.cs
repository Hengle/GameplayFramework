using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poi;

public class UI
{
    public static UI Instance { get; private set; } = new UI();
    private UI() { }
    Dictionary<int, Pawn> PawnDic = new Dictionary<int, Pawn>();
    internal static void AddPawn(Pawn pawn)
    {
        int id = pawn.DataInfo.ID;
        if (Instance.PawnDic.ContainsKey(id))
        {
            Instance.PawnDic[id] = pawn;
        }
        else
        {
            Instance.PawnDic.Add(id, pawn);
        }

    }

    internal static void RemovePawn(Pawn pawn)
    {
        int id = pawn.DataInfo.ID;
        Instance.PawnDic.Remove(id);
    }
}
