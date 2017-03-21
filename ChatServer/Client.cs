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
        public Client(Socket socket):base(socket)
        {
            BeginReceive();
        }  
    }
}