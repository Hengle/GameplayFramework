﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class MonsterInfo:PawnInfo
    {
        public override PawnType PawnType
        {
            get
            {
                return PawnType.Monster;
            }
        }
    }
}
