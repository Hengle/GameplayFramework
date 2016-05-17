using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 宿主接口
    /// </summary>
    public interface iHost
    {
        UnitState State { get; }
        void Log(object message, uint level = 0);
        void LogWarning(object message, uint level = 0);
        void LogError(object message, uint level = 0);
    }
}
