﻿using System;
using System.IO;
using ProtoBuf;

namespace ChatServer
{
    internal class GlobalServerClient:MMONet.Client
    {
        private Server ChatServer;

        public GlobalServerClient(Server server)
        {
            this.ChatServer = server;
        }

        protected override void OnResponse(int key, MemoryStream value)
        {
            base.OnResponse(key, value);
            if (key == ProtoID.GetID<ChildServerBeginWork>()) OnBeginWork(value);
            
        }

        private void OnBeginWork(MemoryStream value)
        {
            var pks = Serializer.Deserialize<ChildServerBeginWork>(value);
            ChatServer.BeginWork(pks.Port);
        }
    }
}