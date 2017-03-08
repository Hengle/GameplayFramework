﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using MMONet;
using ProtoBuf;

namespace ChatServer
{
    public partial class Client
    {
        protected override void OnResponse(ushort key, MemoryStream value)
        {
            if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
            else if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
        }

        private void OnChatMsg(MemoryStream value)
        {
            ChatMsg msg = Serializer.Deserialize<ChatMsg>(value);
            DateTime time = DateTime.Now;
            Heart msg2 = new Heart();
            msg2.Time = time.ToBinary();
            BroadCast(msg);
        }
    }
}