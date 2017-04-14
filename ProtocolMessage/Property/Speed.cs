using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poi
{
    
    public partial class SpeedBase : RangeDataProperty
    {
        public SpeedBase(PropertyType type)
        {
            this.PropertyType = type;
        }

        /// <summary>
        /// 启动速率
        /// </summary>
        [ProtoMember(1)]
        public float StartScale { get; set; } = 0.3f;
    }
}
