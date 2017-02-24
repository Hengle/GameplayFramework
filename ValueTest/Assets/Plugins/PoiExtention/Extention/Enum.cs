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

    /// <summary>
    /// 插值类型
    /// </summary>
    public enum LerpType
    {
        Lerp,
        LerpUnclamped,
        Slerp,
        SlerpUnclamped,
        Null,
    }

    public enum PoiTag
    {
        PlayerStart,
        Pawn,
        Monster,
        Character,
        Map,
    }
}
