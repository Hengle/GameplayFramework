using System;
using System.IO;
using ProtoBuf;

namespace ChatServer
{
    internal class GlobalServerClient:MMONet.Remote
    {
        private Server ChatServer;

        public GlobalServerClient(Server server)
        {
            this.ChatServer = server;
        }

        protected override void Response(int key, MemoryStream value)
        {
            base.Response(key, value);
            if (key == PID<ChildServerBeginWork>.Value) OnBeginWork(value);
            
        }

        private void OnBeginWork(MemoryStream value)
        {
            var pks = Serializer.Deserialize<ChildServerBeginWork>(value);
            ChatServer.BeginWork(pks.Port);
        }
    }
}