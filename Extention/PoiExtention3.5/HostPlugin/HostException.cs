using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 宿主异常
    /// </summary>
    public class HostException:Exception
    {
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        public HostErrorCode Type { get; set; }
    }

    public enum HostErrorCode
    {
        Null,
        Initing,
        Dispose,
    }
}
