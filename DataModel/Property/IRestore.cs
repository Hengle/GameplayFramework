using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 可恢复
    /// </summary>
    public interface IRestore
    {
        #region 恢复
        /// <summary>
        /// 每秒恢复
        /// </summary>
        double RestorePerSecond { get; set; }
        /// <summary>
        /// 距离下次恢复时间
        /// </summary>
        float RestoreCooldownTime { get; set; }
        /// <summary>
        /// 禁用恢复时间
        /// </summary>
        float RestoryDisabledTime { get; set; }
        #endregion
    }
}
