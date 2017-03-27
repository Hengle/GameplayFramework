using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 
    /// </summary>
    public class SpeedBase : RangeDataProperty
    {
        public SpeedBase(PropertyType type)
        {
            this.PropertyType = type;
        }

        /// <summary>
        /// 启动速率
        /// </summary>
        public float StartScale { get; set; } = 0.3f;
    }
}
