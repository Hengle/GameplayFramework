﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class Character:Pawn
    {
        public new CharacterInfo DataInfo
        {
            get
            {
                return dataInfo as CharacterInfo;
            }
        }
    }
}
