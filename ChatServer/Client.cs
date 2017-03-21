using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using MMONet;
using ProtoBuf;

namespace ChatServer
{
    public partial class Client : MMONet.Remote
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
    }
}