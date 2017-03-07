using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoBuf
{
    public class ProtoBufExtention
    {
    }

    /// <summary>
    /// 用于解析报头的编号值，项目中所有ID必须唯一
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtoID : Attribute
    {
        static List<int> already = new List<int>();
        public ProtoID(int id)
        {
            if (already.Contains(id))
            {
                throw new ArgumentException("当前ID冲突");
            }
            ID = id;
        }

        public int ID { get; private set; }
    }
}
