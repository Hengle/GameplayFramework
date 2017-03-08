using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public partial class Client
    {
        /// <summary>
        /// 向所有客户端广播消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void BroadCast<T>(T msg)
        {
            throw new NotImplementedException();
        }
    }
}
