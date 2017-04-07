
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
        if (key == PID<ChatMsg>.Value) OnChatMsg(value);
        else if (key == PID<Heart>.Value) OnHeart(value);
        else if (key == PID<ALogin>.Value) OnALogin(value);
        else if (key == PID<AChildServerAddress>.Value) OnAChildServerAddress(value);
        else if (key == PID<PlayerInfo>.Value) OnPlayerInfo(value);
        else if (key == PID<Quit>.Value) OnCharacterQuit(value);
        else if (key == PID<TransList>.Value) OnTransList(value);
        else if (key == PID<NameChange>.Value) OnNameChange(value);
        else if (key == PID<ModelChange>.Value) OnModelChange(value);
    }

    private void OnModelChange(MemoryStream value)
    {
        var pks = Serializer.Deserialize<ModelChange>(value);
        var ch = GetCharacter(pks.instanceID);
        if (ch && pks.instanceID != Player.InstanceID)
        {
            Vector3 pos = ch.transform.position;
            Quaternion rotation = ch.transform.rotation;

            Poi.CharacterInfo info = ch.DataInfo;
            info.ModelName = pks.ModelName;
            Character.Dic.Remove(pks.instanceID);
            GameObject.Destroy(ch.gameObject);

            var character = Character.CreateCharacter(info);
            character.transform.position = pos;
            character.transform.rotation = rotation;
        }
    }

    private void OnNameChange(MemoryStream value)
    {
        var pks = Serializer.Deserialize<NameChange>(value);
        var ch = GetCharacter(pks.instanceID);
        if (ch && pks.instanceID != Player.InstanceID)
        {
            ch.DataInfo.Name = pks.Name;
            UI.ChangeName(pks.instanceID, pks.Name, true);
        }
    }

    private Character GetCharacter(int id)
    {
        Character.Dic.TryGetValue(id, out Character res);
        return res;
    }

    private void OnTransList(MemoryStream value)
    {
        var pks = Serializer.Deserialize<TransList>(value);
        foreach (var item in pks.transList)
        {
            if (Character.Dic.ContainsKey(item.instanceID))
            {
                Character.Dic[item.instanceID].Move(item.trans);
            }
        }
    }

    private void OnCharacterQuit(MemoryStream value)
    {
        var pks = Serializer.Deserialize<Quit>(value);
        Character.Quit(pks);
    }

    private void OnPlayerInfo(MemoryStream value)
    {
        var pks = Serializer.Deserialize<PlayerInfo>(value);
        Character.CreateCharacter(pks);
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
            if (IsConnected)
            {
                var msg = new Heart()
                {
                    Time = DateTime.Now.ToBinary(),
                };

                Write(msg);
                heartmsgCooldown = 0.5f;
            } 
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

    public override void DisConnect(DisConnectReason resason = DisConnectReason.Active)
    {
        if (ChatServer != null)
        {
            ChatServer.DisConnect(resason);
        }
        base.DisConnect(resason);
    }
}