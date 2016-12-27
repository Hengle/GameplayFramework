using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 刷新类型
    /// </summary>
    [Flags]
    public enum UpdateType
    {
        Update = 0x1,
        LateUpdate = 0x2,
        FixedUpdate = 0x4,
    }
}
