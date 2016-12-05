﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 魔法值
    /// </summary>
    public class MP:BaseProperty
    {
        public override ValueChangedType ChangedType
        {
            get
            {
                return ValueChangedType.TickChanged;
            }
        }

        public override PropertyType PropertyType
        {
            get
            {
                return PropertyType.MP;
            }
        }
    }
}
