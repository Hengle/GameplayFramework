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
        /// <summary>
        /// 插件当前状态
        /// </summary>
        public PluginState PluginState { get; set; }
    }

    /// <summary>
    /// 插件状态
    /// </summary>
    public enum PluginState
    {
        /// <summary>
        /// 插件为空
        /// </summary>
        Null,
        /// <summary>
        /// 正在初始化
        /// </summary>
        Initing,
        /// <summary>
        /// 初始化失败已停止工作
        /// </summary>
        InitErrorAndStop,
        /// <summary>
        /// 初始化失败正在重试
        /// </summary>
        InitErrorAndReIniting,
        /// <summary>
        /// 初始化完成
        /// </summary>
        InitFinish,
        /// <summary>
        /// 开启
        /// </summary>
        Open,
        /// <summary>
        /// 关闭
        /// </summary>
        Close,
        /// <summary>
        /// 已释放
        /// </summary>
        Dispose
    }
}
