using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 宿主接口
    /// </summary>
    public interface IHost
    {
        /// <summary>
        /// 宿主状态
        /// </summary>
        UnitState State { get; }

///关闭缺少注释警告
#pragma warning disable 1591

        void Log(object message, uint level = 0);
        void LogWarning(object message, uint level = 0);
        void LogError(object message, uint level = 0);
    }
}
