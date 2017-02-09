using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 魔法值
    /// </summary>
    public class MP:MaxDataProperty
    {
        public override ValueChangedType ChangedType => ValueChangedType.TickChanged;

        public override PropertyType PropertyType => PropertyType.MP;
    }
}
