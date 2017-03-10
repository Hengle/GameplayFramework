using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ProtoBuf;
using System.Net.Sockets;
using System.Net;
using MMONet;
using System.IO;

public partial class GM
{
    public ChatServer chatServer;

    public void InitNet()
    {
        ProtoID.Init();
        ///连接服务器
        chatServer = new ChatServer();
        chatServer.BeginConnect(IPAddress.Loopback, 40000, callback, chatServer);    
    }

    public void UpdateMesssage()
    {
         chatServer?.Update();
    }

    private void callback(IAsyncResult ar)
    {
        ChatServer cl = ar.AsyncState as ChatServer;
        cl.EndConnect(ar);

        cl.BeginReceive();

        if (cl.IsConnected)
        {
            Heart msg = new Heart();
            msg.Time = DateTime.Now.ToBinary();
            cl.Write(msg);
        }
    }

    public class ChatServer : MMONet.Client
    {
        protected override void OnResponse(ushort key, MemoryStream value)
        {
            if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
            else if (key == ProtoID.GetID<Heart>()) OnHeart(value);
        }

        private void OnHeart(MemoryStream value)
        {
            var msg = Serializer.Deserialize<Heart>(value);
            TimeSpan delta = DateTime.Now - DateTime.FromBinary(msg.Time);
            Debug.Log(delta.TotalMilliseconds);

            Heart msg2 = new Heart();
            msg2.Time = DateTime.Now.ToBinary();
            GM.Instance.chatServer.Write(msg2);
        }

        private void OnChatMsg(MemoryStream value)
        {

        }

        public void Update()
        {
            UpdateMessage();
        }
    }
}