﻿
using System;
using System.IO;
using System.Net;
using MMONet;
using Poi;
using ProtoBuf;
using UnityEngine;

public class GameServer : Remote
{
    public CustomRemote ChatServer { get; private set; }

    protected override void Response(int key, MemoryStream value)
    {
        if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
        else if (key == ProtoID.GetID<Heart>()) OnHeart(value);
        else if (key == ProtoID.GetID<ALogin>()) OnALogin(value);
        else if (key == ProtoID.GetID<AChildServerAddress>()) OnAChildServerAddress(value);
    }

    private void OnAChildServerAddress(MemoryStream value)
    {
        var pks = Serializer.Deserialize<AChildServerAddress>(value);

        if (pks.Address.ContainsKey(ServerType.ChatServer))
        {
            CustomRemote chatServer = new CustomRemote();
            chatServer.BeginConnect(IPAddress.Parse(pks.Address[ServerType.ChatServer].IPString),
                pks.Address[ServerType.ChatServer].Port, (ar) =>
                 {
                     CustomRemote cs = ar.AsyncState as CustomRemote;
                     cs.EndConnect(ar);
                     if (cs.IsConnected)
                     {
                         cs.BeginReceive();

                         ChatServer = cs;
                         ChatServer.OnResponse += Response;

                         var loginMsg = new QLogin()
                         {
                             TEMPID = Player.InstanceID,
                         };

                         ChatServer.Write(loginMsg);
                     }
                     else
                     {

                     }
                 }, chatServer);
        }

    }

    private void OnALogin(MemoryStream value)
    {
        var pks = Serializer.Deserialize<ALogin>(value);

        switch (pks.Server)
        {
            case ServerType.GlobalServer:
                if (pks.Result == LoginResult.Success)
                {
                    Player.InstanceID = pks.InstanceID;
                    PoiLog.Log(pks.Note);
                    gM.LoginPlayer();
                }
                break;
            case ServerType.ChatServer:
                if (pks.Result == LoginResult.Success)
                {
                    PoiLog.Log("聊天服务器登陆成功");
                }
                break;
            default:
                break;
        }
    }

    private void OnHeart(MemoryStream value)
    {
        var msg = Serializer.Deserialize<Heart>(value);
        TimeSpan delta = DateTime.Now - DateTime.FromBinary(msg.Time);

        GM.Delay =Mathf.Lerp(GM.Delay, (float)delta.TotalMilliseconds / 2,0.5f);
    }

    private void OnChatMsg(MemoryStream value)
    {
        var pks = Serializer.Deserialize<ChatMsg>(value);
        GM.RecieveChat(pks);
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        ChatServer?.Update(deltaTime);

        if (heartmsgCooldown <= 0)
        {
            var msg = new Heart()
            {
                Time = DateTime.Now.ToBinary(),
            };

            Write(msg);
            heartmsgCooldown = 0.1;
        }
        else
        {
            heartmsgCooldown -= deltaTime;
        }
        
    }

    double heartmsgCooldown = 0;
    private GM gM;

    public GameServer(GM gM)
    {
        this.gM = gM;
    }

    public override void Dispose()
    {
        if (ChatServer!=null)
        {
            ChatServer.Dispose();
        }

        base.Dispose();
    }
}