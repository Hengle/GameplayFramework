using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using MMONet;
using ProtoBuf;

namespace ChatServer
{
    public partial class Client : Remote4Server
    {
        private Server server;

        public Client(Socket socket):base(socket)
        {
            BeginReceive();
        }

        public Client(Socket socket, Server server) : this(socket)
        {
            this.server = server;
        }

        public override void DisConnect(DisConnectReason resason = DisConnectReason.Active)
        {
            RemoveRemote(this);

            base.DisConnect(resason);
        }
    }
}