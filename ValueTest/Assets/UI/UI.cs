using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poi;

public class UI
{
    public static UI Instance { get; private set; } = new UI();

    public static Dictionary<int, ITarget> PawnDic => Instance.pawnDic;

    private UI() { }
    Dictionary<int, ITarget> pawnDic = new Dictionary<int, ITarget>();
    internal static void AddPawn(ITarget pawn)
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

    internal static void RemovePawn(ITarget pawn)
    {
        int id = pawn.ID;
        Instance.pawnDic.Remove(id);
    }
}
