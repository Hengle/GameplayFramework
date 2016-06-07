using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface iPlugin:IDisposable
    {
        /// <summary>
        /// 当前插件状态
        /// </summary>
        UnitState State { get; }
    }
}
