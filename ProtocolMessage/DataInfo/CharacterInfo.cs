
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poi
{
    [ProtoInclude(30, typeof(PlayerInfo))]
    [ProtoContract(Name ="50002")]
    public class CharacterInfo:PawnInfo
    {
        public override PawnType PawnType
        {
            get
            {
                return PawnType.Charater;
            }
        }
    }
}
