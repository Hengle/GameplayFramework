
using System;
using System.IO;
using Poi;
using ProtoBuf;
using UnityEngine;

public class GameServer : MMONet.Remote
{
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
    }

    private void OnALogin(MemoryStream value)
    {
        var pks = Serializer.Deserialize<ALogin>(value);
        if (pks.Result == LoginResult.Success)
        {
            Player.InstanceID = pks.InstanceID;
            PoiLog.Log(pks.Note);
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
}