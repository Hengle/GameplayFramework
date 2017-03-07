using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Reflection;

namespace ProtoBuf
{
    public class ProtoBufExtention
    {
    }

    /// <summary>
    /// 用于解析报头的编号值，项目中所有ID必须唯一
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtoIDAttribute : Attribute
    {
        /// <summary>
        /// 报头类型ID所占字节长度
        /// </summary>
        public const int Length = 2;
        static List<ushort> already = new List<ushort>();
        public ProtoIDAttribute(ushort id)
        {
            if (already.Contains(id))
            {
                throw new ArgumentException("当前ID冲突");
            }
            ID = id;
        }

        public ushort ID { get; private set; }
    }

    public class ProtoID
    {
        public static ushort GetID<T>()
        {
            return 0;
        }

        public static void Init(string assemblyName = "ProtocolMessage")
        {
            var ssss = Assembly.GetCallingAssembly();
            var assList = AppDomain.CurrentDomain.GetAssemblies();
            if (string.IsNullOrEmpty(assemblyName))
            {
                assemblyName = "ProtocolMessage";
            }

            ///定义消息的程序集
            var MSGassembly = assList.FirstOrDefault(res => res.GetName().Name == assemblyName);
            if (MSGassembly == null)
            {
                MSGassembly = Assembly.Load(assemblyName);
            }

            Init(MSGassembly);
        }

        public static void Init(Assembly assembly)
        {
            var types = assembly.GetTypes();
            Type idtype = typeof(ProtoIDAttribute);
            Type contracttype = typeof(ProtoContractAttribute);
            foreach (var item in types)
            {
                var res = item.GetCustomAttributes(contracttype, true);
                if (res != null && res.Length > 0)
                {
                    ProtoIDAttribute attribute = (ProtoIDAttribute)item.GetCustomAttributes(idtype, true)?[0];
                    if (attribute != null)
                    {
                        map.Add(item.GUID, attribute.ID);
                    }
                }

            }
        }

        static Dictionary<Guid, ushort> map = new Dictionary<Guid, ushort>();
        //static Dictionary<>
    }
}
