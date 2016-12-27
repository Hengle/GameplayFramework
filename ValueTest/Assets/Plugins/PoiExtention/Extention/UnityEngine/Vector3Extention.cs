using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class Vector3Extention
    {
        static readonly Vector3 zeroY = new Vector3(1, 0, 1);

        [Obsolete("可能会导致装箱?不确定性能损失",false)]
        public static Vector3 ZeroY(this Vector3 v)
        {
            return Vector3.Scale(v,zeroY);
        }
    }
}
