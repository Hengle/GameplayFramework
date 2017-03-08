using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Reflection;
using System.IO;

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
            Guid guid = typeof(T).GUID;
            if (map.ContainsKey(guid))
            {
                return map[guid];
            }
            return 0;
        }

        /// <summary>
        /// 映射协议程序集内类型和协议ID
        /// </summary>
        /// <param name="assemblyName"></param>
        public static void Init(string assemblyName = "ProtocolMessage")
        { 
            if (string.IsNullOrEmpty(assemblyName))
            {
                assemblyName = "ProtocolMessage";
            }

            var assList = AppDomain.CurrentDomain.GetAssemblies();
            ///定义消息的程序集
            var MSGassembly = assList.FirstOrDefault(res => res.GetName().Name == assemblyName);
            if (MSGassembly == null)
            {
                var curAssembly = Assembly.GetExecutingAssembly();
                string path = Path.Combine(Path.GetDirectoryName(curAssembly.Location), assemblyName);
                var ext= Path.GetExtension(path);
                const string dll = ".dll";
                if (ext != dll)
                {
                    path = Path.ChangeExtension(path, dll);
                }
                MSGassembly = Assembly.LoadFrom(path);
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
                        if (map.ContainsValue(attribute.ID))
                        {
                            throw new ArgumentException(item.ToString() + "协议ID发生冲突");
                        }
                        map.Add(item.GUID, attribute.ID);
                    }
                }

            }
        }

        static Dictionary<Guid, ushort> map = new Dictionary<Guid, ushort>();
    }
}
