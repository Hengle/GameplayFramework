using System;
using System.IO;
using System.Net.Sockets;
using ProtoBuf;
using System.Collections.Generic;
using System.Net;

namespace GlobalServer
{
    public class GameClient : MMONet.Remote
    {
        public static Dictionary<int, GameClient> clientDic = new Dictionary<int, GameClient>();

        private Server server;

        public GameClient(Socket socket) : base(socket)
        {
        }

        public GameClient(Socket socket, Server server) : this(socket)
        {
            this.server = server;
        }

        protected override void Response(int key, MemoryStream value)
        {
            if (key == ProtoID.GetID<QLogin>()) OnQLogin(value);
            
        }

        private void OnQLogin(MemoryStream value)
        {
            var pks = Serializer.Deserialize<QLogin>(value);
            int id = Poi.ID.GetGlobalID();

            lock (clientDic)
            {
                clientDic.Add(id, this);
                server.UnknownClient.Remove(this);
            }

            var msg = new ALogin()
            {
                InstanceID = id,
                Note = $"登陆服务器成功！",
                Result = LoginResult.Success
            };

            Console.WriteLine($"客户端{pks.account}登陆，分配临时ID：{id}。当前客户端数量：{clientDic.Count}。");

            Write(msg);

            SendChildServerAddresss();
        }

        /// <summary>
        /// 发送子服务器地址
        /// </summary>
        private void SendChildServerAddresss()
        {
            var msg2 = new AChildServerAddress();
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, Port.ChatServerListen);
            msg2.Address[ServerType.ChatServer] = new IP()
            {
                IPString = IPAddress.Loopback.ToString(),
                Port = Port.ChatServerListen
            };

            Write(msg2);
        }
    }
}