using System;
using System.IO;
using System.Net.Sockets;
using ProtoBuf;
using System.Collections.Generic;
using System.Net;
using MMONet;

namespace GlobalServer
{
    public class GameClient : MMONet.Remote
    {
        public static Dictionary<int, GameClient> clientDic = new Dictionary<int, GameClient>();

        private Server server;

        public int InstanceID { get; private set; }
        public string Account { get; private set; }

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
            if (key == ProtoID.GetID<Heart>()) OnlyReturn(key,value);
        }

        private void OnlyReturn(int key,MemoryStream value)
        {
            Write(key, value);
        }

        private void OnQLogin(MemoryStream value)
        {
            var pks = Serializer.Deserialize<QLogin>(value);
            InstanceID = Poi.ID.GetGlobalID();
            Account = pks.account;

            lock (clientDic)
            {
                clientDic.Add(InstanceID, this);
                server.UnknownClient.Remove(this);
            }

            var msg = new ALogin()
            {
                InstanceID = InstanceID,
                Note = $"登陆服务器成功！",
                Result = LoginResult.Success,
                Server = ServerType.GlobalServer,
            };

            Console.WriteLine($"客户端{pks.account}登陆，分配临时ID：{InstanceID}。当前客户端数量：{clientDic.Count}。");

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

        public override void DisConnect(DisConnectReason resason = DisConnectReason.Active)
        {
            Remove(this);

            base.DisConnect(resason);
        }

        public static void Remove(GameClient client)
        {
            lock (clientDic)
            {
                clientDic.Remove(client.InstanceID);
                Console.WriteLine($"客户端{client.Account}退出，ID：{client.InstanceID}。当前客户端数量：{clientDic.Count}。");
            }
        }
    }
}