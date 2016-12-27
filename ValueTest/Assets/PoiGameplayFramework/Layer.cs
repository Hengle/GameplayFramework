using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public enum Layer
    {
        Default = 0,
        TransparentFX,
        IgnoreRaycast,

        Water = 4,
        UI = 5,

        Character = 10,
        Monster,
        Floor = 21,
    }
}
