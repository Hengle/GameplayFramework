
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
        Debug.Log(delta.TotalMilliseconds);

        Heart msg2 = new Heart();
        msg2.Time = DateTime.Now.ToBinary();
        GM.Instance.Server.Write(msg2);
    }

    private void OnChatMsg(MemoryStream value)
    {

    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        ChatServer?.Update(deltaTime);
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