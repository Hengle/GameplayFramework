using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ProtoBuf;
using System.Net.Sockets;
using System.Net;
using MMONet;
using System.IO;

public partial class GM
{
    public GameServer Server;

    public void InitNet()
    {
        ProtoID.Init();
        ///连接服务器
        Server = new GameServer();
        Server.BeginConnect(IPAddress.Loopback, Port.GlobalListen, callback, Server);    
    }

    public void UpdateMesssage(double delta)
    {
         Server?.Update(delta);
    }

    private void callback(IAsyncResult ar)
    {
        GameServer cl = ar.AsyncState as GameServer;
        cl.EndConnect(ar);

        cl.BeginReceive();

        if (cl.IsConnected)
        {
            QLogin msg = new QLogin() { account = "tEXT"/*SystemInfo.deviceUniqueIdentifier*/
            };

            cl.Write(msg);
        }
    }

}