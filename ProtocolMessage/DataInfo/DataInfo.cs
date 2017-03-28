using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;
using ProtoBuf;

namespace Poi
{
    /// <summary>
    /// 数据信息，内存模型
    /// </summary>
    [ProtoContract(Name = "50000")]
    [ProtoInclude(30, typeof(PawnInfo))]
    public class DataInfo
    {
        public static implicit operator bool(DataInfo data)
        {
            return data != null;
        }
    }
}
