using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public interface IUITarget
    {
        int ID { get; }
        Vector3 ScreenPosition { get; }
    }

}