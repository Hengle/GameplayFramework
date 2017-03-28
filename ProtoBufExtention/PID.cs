using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ProtoBuf
{
    public static class PID
    {
        /// <summary>
        /// 报头类型ID所占字节长度
        /// </summary>
        public const int Length = sizeof(int);
    }

    public static class PID<T>
    {
        static int? id = null;
        public static int Value
        {
            get
            {
                if (id == null)
                {
                    Type tartype = typeof(T);
                    ProtoContractAttribute res =
                    tartype.GetCustomAttributes(typeof(ProtoContractAttribute), true).FirstOrDefault()
                    as ProtoContractAttribute;

                    if (res == null)
                    {
                        id = 0;
                        throw new ArgumentException(tartype.ToString() + "没有协议特性ProtoContract标记");
                    }
                    else
                    {
                        int resid;
                        if (int.TryParse(res.Name, out resid))
                        {
                            if (resid <= 0)
                            {
                                throw new ArgumentException(tartype.ToString() + "协议ID未能解析为正整数");
                            }
                            else
                            {
                                if (PIDFilter.AlreadyHava.ContainsKey(resid))
                                {
                                    throw new ArgumentException("当前类型："+ tartype.FullName
                                        +"与已知类型："+ PIDFilter.AlreadyHava[resid] + "协议ID发生冲突");
                                }
                                else
                                {
                                    id = resid;
                                    PIDFilter.AlreadyHava[resid] = tartype.FullName;
                                }
                            }
                        }
                        else
                        {
                            throw new ArgumentException(tartype.ToString()+ "协议ID未能解析为正整数");
                        }
                    }
                }
                
                return id ?? 0;               
            }
        }
    }

    class PIDFilter
    {
        private static Dictionary<int, string> alreadyHava = new Dictionary<int, string>();

        public static Dictionary<int, string> AlreadyHava { get => alreadyHava; set => alreadyHava = value; }
    }

    //public class ID
    //{
    //    static Dictionary<string, int> map = new Dictionary<string, int>();
    //    public static int Get<T>()
    //    {
    //        Type tartype = typeof(T);
    //        string name = tartype.FullName;
    //        if (!map.ContainsKey(name))
    //        {
    //            ProtoContractAttribute res =
    //                tartype.GetCustomAttributes(typeof(ProtoContractAttribute), true).FirstOrDefault()
    //                as ProtoContractAttribute;

    //            if (res == null)
    //            {
    //                map[name] = 0;
    //            }
    //            else
    //            {
    //                int id;
    //                if (int.TryParse(res.Name, out id))
    //                {
    //                    if (id !=0 && map.ContainsValue(id))
    //                    {
    //                        throw new ArgumentException(name + "协议ID发生冲突");
    //                    }
    //                    map[name] = id;
    //                }
    //                else
    //                {
    //                    map[name] = 0;
    //                }
    //            }
    //        }
    //        return map[name];
    //    }
    //}
}
