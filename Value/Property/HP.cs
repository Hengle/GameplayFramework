using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class HP:Property
    {
        public override ValueChangedType ChangedType
        {
            get
            {
                return ValueChangedType.TickChanged;
            }
        }
    }
}
