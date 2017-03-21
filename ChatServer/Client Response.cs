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
            if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
            else if (key == ProtoID.GetID<Heart>()) OnHeart(value);
            else if (key == ProtoID.GetID<QLogin>()) OnQLogin(value);
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
            Write(1001,value);
        }

        int i = 0;
        private void OnChatMsg(MemoryStream value)
        {
            i++;
            ChatMsg msg = Serializer.Deserialize<ChatMsg>(value);
            Console.WriteLine(msg.CharacterID + "----" + msg.Context + "----收到消息个数-------" + i);
        }
    }
}