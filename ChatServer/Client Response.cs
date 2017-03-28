using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using MMONet;
using ProtoBuf;

namespace ChatServer
{
    public partial class Client
    {
        public static Dictionary<int, Client> ClientDic { get; private set; }
                                                    = new Dictionary<int, Client>();
        public int InstanceID { get; private set; }

        protected override void Response(int key, MemoryStream value)
        {
            if (key == PID<ChatMsg>.Value) OnChatMsg(key,value);
            else if (key == PID<QLogin>.Value) OnQLogin(value);
        }

        private void OnQLogin(MemoryStream value)
        {
            var pks = Serializer.Deserialize<QLogin>(value);
            lock (ClientDic)
            {
                ClientDic.Add(pks.TEMPID, this);
                InstanceID = pks.TEMPID;
                server.UnknownClientList.Remove(this);

                var msg = new ALogin()
                {
                    Result = LoginResult.Success,
                    Server = ServerType.ChatServer,
                };

                Write(msg);

                Console.WriteLine($"客户端{InstanceID}登陆。当前客户端数量：{ClientDic.Count}。");
            }
        }

        private void OnHeart(MemoryStream value)
        {
            Write(100,value);
        }

        private void OnChatMsg(int key,MemoryStream value)
        {
            var msg = CombineIDMsg(key, value);
            BroadCast(msg,new int[] { InstanceID });
        }
    }
}