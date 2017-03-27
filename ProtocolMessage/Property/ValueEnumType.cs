using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 属性类型
    /// </summary>
    public enum PropertyType
    {
        Default = 0,
        HP,
        MP,
        /// <summary>
        /// 体力
        /// </summary>
        Vitality,
        /// <summary>
        /// 能量
        /// </summary>
        Energy,
        /// <summary>
        /// 攻击力
        /// </summary>
        ATK,
        WalkSpeed,
        RunSpeed,
    }




    /// <summary>
    /// 数据变动类型
    /// </summary>
    public enum ValueChangedType
    {
        /// <summary>
        /// 恒定不变的
        /// </summary>
        Const = 0,
        /// <summary>
        /// 特殊控制而改变的
        /// </summary>
        AlterChanged,
        /// <summary>
        /// 在Tick中变动的数值属性（可恢复的 生命值，可浮动的 概率）
        /// </summary>
        TickChanged,
    }
}
