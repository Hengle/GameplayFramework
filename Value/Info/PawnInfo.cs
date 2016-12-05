﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class PawnInfo:DataInfo
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        public virtual PawnType PawnType { get { return PawnType.Pawn; } }
    }
}
