﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 生命值
    /// </summary>
    public class HP:BaseProperty
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
                return PropertyType.HP;
            }
        }
    }
}
