using System;
using System.IO;
using System.Net.Sockets;
using ProtoBuf;
using System.Collections.Generic;
using System.Net;
using MMONet;
using Poi;

namespace GlobalServer
{
    public class GameClient : Remote4Server
    {
        private Server server;
        public string Account { get; private set; }
        public PlayerInfo CharacterInfo { get; private set; }
        public Trans Trans { get; internal set; }

        public GameClient(Socket socket) : base(socket)
        {
        }

        public GameClient(Socket socket, Server server) : this(socket)
        {
            this.server = server;
        }

        protected override void Response(int key, MemoryStream value)
        {
            if (key == PID<QLogin>.Value) OnQLogin(value);
            if (key == PID<Heart>.Value) OnlyReturn(key,value);
            if (key == PID<PlayerInfo>.Value) OnSavaCharacter(value);
            if (key == PID<TransSync>.Value) OnTransSync(value);
        }

        private void OnTransSync(MemoryStream value)
        {
            var transync = Serializer.Deserialize<TransSync>(value);
            if (transync.instanceID == InstanceID)
            {
                Trans = transync.trans;
            }
        }

        private void OnSavaCharacter(MemoryStream value)
        {
            CharacterInfo = Serializer.Deserialize<PlayerInfo>(value);
            BroadCastExceptSelf(CharacterInfo);

            foreach (var item in ClientDic)
            {
                GameClient c = item.Value as GameClient;
                if (item.Key != InstanceID)
                {
                    Write(c.CharacterInfo);
                }
            }
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

            lock (ClientDic)
            {
                AddRemote(this);
                server.UnknownClient.Remove(this);
            }

            var msg = new ALogin()
            {
                InstanceID = InstanceID,
                Note = $"登陆服务器成功！",
                Result = LoginResult.Success,
                Server = ServerType.GlobalServer,
            };

            Write(msg);

            SendChildServerAddresss();
        }

        /// <summary>
        /// 发送子服务器地址
        /// </summary>
        private void SendChildServerAddresss()
        {
            var msg = new AChildServerAddress();
            msg.Address[ServerType.ChatServer] = new IP()
            {
                IPString = (server.CurrentIP ?? IPAddress.Loopback).ToString(),
                Port = Port.ChatServerListen
            };

            Write(msg);
        }

        public override void DisConnect(DisConnectReason resason = DisConnectReason.Active)
        {
            var msg = new Quit()
            {
                InstanceID = InstanceID,
            };
            BroadCastExceptSelf(msg);

            RemoveRemote(this);
            base.DisConnect(resason);
        }
    }
}