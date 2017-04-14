using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;
using ProtoBuf;

namespace Poi
{
    /// <summary>
    /// 角色类型
    /// </summary>
    public enum PawnType
    {
        Pawn = 0,
        NPC,
        Monster,
        Charater,
        Player,
    }


    /// <summary>
    /// 数据信息，内存模型
    /// </summary>
    [ProtoContract(Name = "12100010")]
    [ProtoInclude(100, typeof(PawnInfo))]
    public class DataInfo
    {
        public static implicit operator bool(DataInfo data)
        {
            return data != null;
        }
    }

    [ProtoContract(Name = "12100020")]
    [ProtoInclude(200, typeof(CharacterInfo))]
    public partial class PawnInfo { }

    [ProtoContract(Name = "12100030")]
    [ProtoInclude(300, typeof(PlayerInfo))]
    public partial class CharacterInfo { }

    [ProtoContract(Name = "12100040")]
    public partial class PlayerInfo { }
}
