using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ProtoBuf;
using System.Net.Sockets;
using System.Net;

public partial class GM
{
    public void InitNet()
    {
        ///连接服务器
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        TcpClient client = new TcpClient(AddressFamily.InterNetwork);
        IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 40000);
        client.BeginConnect(IPAddress.Loopback, 40000, callback, client);


        
    }

    private void callback(IAsyncResult ar)
    {
        TcpClient cl = ar.AsyncState as TcpClient;
        cl.EndConnect(ar);

        
    }
}