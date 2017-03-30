using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ProtoBuf
{
    /// <summary>
    /// 聊天消息
    /// </summary>
    [ProtoContract( Name = "1000")]
    public class ChatMsg
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
        public string Context { get; set; }
    }

    [ProtoContract(Name = "1001")]
    public class Trans
    {
        [ProtoMember(2)]
        public float x;
        [ProtoMember(3)]
        public float y;
        [ProtoMember(4)]
        public float z;
        [ProtoMember(5)]
        public float qx;
        [ProtoMember(6)]
        public float qy;
        [ProtoMember(7)]
        public float qz;
        [ProtoMember(8)]
        public float qw;    
    }

    [ProtoContract(Name = "1002")]
    public class TransSync
    {
        [ProtoMember(1)]
        public int instanceID;
        [ProtoMember(2)]
        public Trans trans;
    }

    [ProtoContract(Name = "1003")]
    public class TransList
    {
        [ProtoMember(1)]
        public List<TransSync> transList = new List<TransSync>();
    }
}
