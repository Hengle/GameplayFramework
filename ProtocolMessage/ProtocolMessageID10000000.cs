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




    [ProtoContract(Name = "10000001")]
    public class QLogin
    {
        [ProtoMember(1)]
        public string account;
        [ProtoMember(2)]
        public int TEMPID;
        [ProtoMember(3)]
        public string Note;
    }

    [ProtoContract(Name = "10000002")]
    public class ALogin
    {
        [ProtoMember(1)]
        public LoginResult Result;
        [ProtoMember(2)]
        public int InstanceID;
        [ProtoMember(3)]
        public string Note;
        [ProtoMember(4)]
        public ServerType Server;
    }

    public enum LoginResult
    {
        Success = 0,
        Error = 1,
    }

    [ProtoContract(Name = "10000003")]
    public class QChildServerAddress
    {
        
    }

    [ProtoContract(Name = "10000004")]
    public class AChildServerAddress
    {
        [ProtoMember(1)]
        public Dictionary<ServerType, IP> Address
            = new Dictionary<ServerType, IP>();
    }

    [ProtoContract(Name = "10000005")]
    public class IP
    {
        [ProtoMember(1)]
        public string IPString;
        [ProtoMember(2)]
        public int Port;
    }

    [ProtoContract(Name = "10000006")]
    public class Quit
    {
        [ProtoMember(1)]
        public LoginResult Result;
        [ProtoMember(2)]
        public int InstanceID;
        [ProtoMember(3)]
        public string Note;
        [ProtoMember(4)]
        public ServerType Server;
    }

    /// <summary>
    /// 聊天消息
    /// </summary>
    [ProtoContract(Name = "10000010")]
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



    /// <summary>
    /// 聊天消息
    /// </summary>
    [ProtoContract(Name = "10000011")]
    public class HeartEX
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
        /// <summary>
        /// 聊天内容
        /// </summary>
        [ProtoMember(3)]
        public double ServerTime { get; set; }
    }
}
