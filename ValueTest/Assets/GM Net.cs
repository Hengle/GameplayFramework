﻿using System;
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
using System.Collections;

public partial class GM
{
    public GameServer Server;

    public static LineMode LineMode { get; private set; } = LineMode.Single;

    public static void Login(IPAddress ip)
    {
        if (Instance.Server != null && Instance.Server.IsConnected)
        {
            return;
        }
        ///连接服务器
        Instance.Server = new GameServer(Instance);
        if (ip == null)
        {
            ip = IPAddress.Loopback;
        }

        Instance.Server.BeginConnect(ip, Port.GlobalListen, Instance.callback, Instance.Server);

        LineMode = LineMode.Offline;
    }

    public static void Logout()
    {
        if (Instance.Server != null)
        {
            Instance.Server.DisConnect(DisConnectReason.Active);
            Instance.Server.Dispose();
            Instance.Server = null;
        }

        Delay = 0;
        GM.LineMode = LineMode.Offline;
        Character.ChangeMode(LineMode.Offline);
    }

    public void InitNet()
    {
           
    }

    public void UpdateMesssage(double delta)
    {
         Server?.Update(delta);
    }

    private void callback(IAsyncResult ar)
    {
        GameServer cl = ar.AsyncState as GameServer;
        cl.EndConnect(ar);

        if (cl.IsConnected)
        {
            cl.BeginReceive();

            LineMode = LineMode.Online;

            QLogin msg = new QLogin()
            {
                account = "tEXT"/*SystemInfo.deviceUniqueIdentifier*/
            };
            Instance.Server.Write(msg);
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
        Player.Instance.SendTrans();
        Server.Write(Player.DataInfo);
    }

    internal static void WriteToServer<T>(T msg)
    {
        if (Instance.Server != null && Instance.Server.IsConnected)
        {
            Instance.Server.Write(msg);
        }
    }

    public static void RecieveChat(ChatMsg pks)
    {
        string name = GetName(pks.CharacterID);
        UI.ShowChatMsg(name, pks.Context);
    }

    private static string GetName(int characterID)
    {
        if (Character.Dic.ContainsKey(characterID))
        {
            return Character.Dic[characterID].Name;
        }
        return characterID.ToString();
    }
}