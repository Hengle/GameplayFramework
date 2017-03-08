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
        private void OnResponse(ushort key, MemoryStream value)
        {
            if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
            else if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
        }

        private void OnChatMsg(MemoryStream value)
        {
            ChatMsg msg = Serializer.Deserialize<ChatMsg>(value);
            BroadCast(msg);
        }
    }
}