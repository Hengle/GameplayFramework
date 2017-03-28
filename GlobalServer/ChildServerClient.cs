using System;
using System.IO;
using System.Net.Sockets;
using ProtoBuf;

namespace GlobalServer
{
    public class ChildServerClient : MMONet.Remote
    {
        private Server server;

        public ChildServerClient(Socket socket) : base(socket)
        {
        }

        public ChildServerClient(Socket socket, Server server) : this(socket)
        {
            this.server = server;
        }

        protected override void Response(int key, MemoryStream value)
        {
            base.Response(key, value);
            if (key == PID<ServerLogin>.Value) OnServerLogin(value);
            //else if (key == ProtoID.GetID<Heart>()) OnHeart(value);
        }

        private void OnServerLogin(MemoryStream value)
        {
            var pks = Serializer.Deserialize<ServerLogin>(value);
            switch (pks.Type)
            {
                case ServerType.GlobalServer:
                    break;
                case ServerType.ChatServer:
                    server.ChatServer = this;
                    server.UnknownClient.Remove(this);
                    Console.WriteLine("ChatServer Success Start!");


                    var msg = new ChildServerBeginWork() { Port = Port.ChatServerListen };
                    Write(msg);
                    break;
                default:
                    break;
            }
        }
    }
}