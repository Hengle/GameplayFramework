using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ProtoBuf;

namespace ProtoBuf
{


    public enum ServerType
    {
        GlobalServer = 0,
        ChatServer = 1,

    }


    /// <summary>
    /// 聊天消息
    /// </summary>
    [ProtoContract, ProtoID(100)]
    public class Heart
    {
        /// <summary>
        /// 角色实例ID
        /// </summary>
        [ProtoMember(1)]
        public int CharacterID { get; set; }
        /// <summary>
        /// 聊天内容
        /// </summary>
        [ProtoMember(2)]
        public long Time { get; set; }
    }


    [ProtoContract, ProtoID(101)]
    public class QLogin
    {
        [ProtoMember(1)]
        public string account;
        [ProtoMember(2)]
        public int TEMPID;
        [ProtoMember(3)]
        public string Note;
    }

    [ProtoContract, ProtoID(102)]
    public class ALogin
    {
        [ProtoMember(1)]
        public LoginResult Result;
        [ProtoMember(2)]
        public int InstanceID;
        [ProtoMember(3)]
        public string Note;
    }

    public enum LoginResult
    {
        Success = 0,
        Error = 1,
    }

    [ProtoContract, ProtoID(103)]
    public class QChildServerAddress
    {
        
    }

    [ProtoContract, ProtoID(104)]
    public class AChildServerAddress
    {
        [ProtoMember(1)]
        public Dictionary<ServerType, IP> Address
            = new Dictionary<ServerType, IP>();
    }

    [ProtoContract, ProtoID(105)]
    public class IP
    {
        [ProtoMember(1)]
        public string IPString;
        [ProtoMember(2)]
        public int Port;
    }
}
