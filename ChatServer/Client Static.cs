﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMONet;
using ProtoBuf;

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

        public static void BroadCast(MemoryStream msg,IList<int> exceptClient = null)
        {
            ArraySegment<byte> sendbytes = new ArraySegment<byte>(msg.ToArray(), 0, (int)msg.Length);
            foreach (var item in ClientDic)
            {
                if (exceptClient != null && exceptClient.Contains(item.Key))
                {
                    continue;
                }
                item.Value.Write(sendbytes);
            }
        }

        public override void DisConnect(DisConnectReason resason = DisConnectReason.Active)
        {
            Remove(this);

            base.DisConnect(resason);
        }

        public static void Remove(Client client)
        {
            lock(ClientDic)
            {
                ClientDic.Remove(client.InstanceID);
                Console.WriteLine($"客户端{client.InstanceID}退出。当前客户端数量：{ClientDic.Count}。");
            }
        }
    }
}
