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
        /// 属性刷新类型
        /// </summary>
        public virtual ValueChangedType ChangedType { get; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public virtual PropertyType PropertyType { get; protected set; }
    }
}
