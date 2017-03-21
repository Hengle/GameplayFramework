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
        protected override void OnResponse(int key, MemoryStream value)
        {
            if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
            else if (key == ProtoID.GetID<Heart>()) OnHeart(value);
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