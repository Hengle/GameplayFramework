using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poi
{
    public partial class PlayerInfo:CharacterInfo
    {
        public override PawnType PawnType
        {
            get
            {
                return PawnType.Player;
            }
        }
    }
}
