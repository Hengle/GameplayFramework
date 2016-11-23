using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 数值
    /// </summary>
    public class DataProperty
    {
        /// <summary>
        /// 许可的最大值
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        /// 许可的最小值
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public double Current { get; set; }

        #region 恢复
        /// <summary>
        /// 每秒恢复
        /// </summary>
        public double RestorePerSecond { get; set; }
        /// <summary>
        /// 距离下次恢复时间
        /// </summary>
        public float RestoreCooldownTime { get; set; }
        /// <summary>
        /// 禁用恢复时间
        /// </summary>
        public float RestoryDisabledTime { get; set; }
        #endregion
    }
}
