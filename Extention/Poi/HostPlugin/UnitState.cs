using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 插件状态，包装一个状态类用于传入可迭代方法中
    /// </summary>
    public class UnitState
    {
        public PluginState State { get; set; }
    }

    /// <summary>
    /// 插件状态
    /// </summary>
    public enum PluginState
    {
        Null,
        Initing,
        InitErrorAndStop,
        InitErrorAndReIniting,
        InitFinish,
        Open,
        Close,
        Dispose
    }
}
