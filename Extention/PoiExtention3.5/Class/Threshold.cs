using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 阈值
    /// </summary>
    public struct Threshold
    {
        /// <summary>
        /// 上界
        /// </summary>
        public readonly float Upper;
        /// <summary>
        /// 下界
        /// </summary>
        public readonly float Lower;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="l">下界</param>
        /// <param name="u">上界</param>
        public Threshold(float l, float u)
        {
            Lower = l;
            Upper = u;
        }

        /// <summary>
        /// 是不是在界限内
        /// </summary>
        /// <param name="v">现有值</param>
        /// <param name="args">调整幅度</param>
        /// <param name="level">调整级别</param>
        /// <returns><![CDATA[return v >= (Lower - args * level) || v <= (Upper + args * level)]]></returns>
        public bool IsInValue(float v, int args = 0, int level = 0)
        {
            return v >= (Lower - args * level) || v <= (Upper + args * level);
        }

        /// <summary>
        /// 是否小于下界
        /// </summary>
        /// <param name="v"></param>
        /// <param name="args"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool IsLessLower(float v, int args = 0, int level = 0)
        {
            return v <= (Lower - args * level);
        }

        /// <summary>
        /// 是否大于上界
        /// </summary>
        /// <param name="v"></param>
        /// <param name="args"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool IsGreaterUpper(float v, int args = 0, int level = 0)
        {
            return v >= (Upper + args * level);
        }
    }
}
