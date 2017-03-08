using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using MMONet;
using ProtoBuf;

namespace ChatServer
{
    public partial class Client : MMOClient
    {
        public Client(Socket socket):base(socket)
        {
            BeginReceive();
        }

        internal void Update(double deltaTime)
        {
            UpdateMessage();
        }

    }
}