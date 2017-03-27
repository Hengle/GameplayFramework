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
using Poi;

public partial class GM
{
    public GameServer Server;

    public static void Login(IPAddress ip)
    {
        ///连接服务器
        Instance.Server = new GameServer(Instance);
        if (ip == null)
        {
            ip = IPAddress.Loopback;
        }
        Instance.Server.BeginConnect(ip, Port.GlobalListen, Instance.callback, Instance.Server);
    }

    public void InitNet()
    {
        ProtoID.Init();       
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

    internal static void SendChat(string obj)
    {
        if (Instance.Server != null)
        {
            var msg = new ChatMsg()
            {
                CharacterID = Player.InstanceID,
                Context = obj,
            };
            Instance.Server.ChatServer.Write(msg);
        }
    }

    public void LoginPlayer()
    {
        Server.Write((Poi.PawnInfo)Player.DataInfo);
    }

    public static void RecieveChat(ChatMsg pks)
    {
        string name = GetName(pks.CharacterID);
        UI.ShowChatMsg(name, pks.Context);
    }

    private static string GetName(int characterID)
    {
        return characterID.ToString();
    }
}