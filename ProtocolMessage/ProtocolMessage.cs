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
        [ProtoMember(2)]
        public double ServerTime;
    }

    [ProtoContract(Name = "1004")]
    public class NameChange
    {
        [ProtoMember(1)]
        public int instanceID;
        [ProtoMember(2)]
        public string Name;
    }

    [ProtoContract(Name = "1005")]
    public class ModelChange
    {
        [ProtoMember(1)]
        public int instanceID;
        [ProtoMember(2)]
        public string ModelName;
    }

    [ProtoContract(Name = "1006")]
    public class InputCMD
    {
        /// <summary>
        /// 跳跃命令
        /// </summary>
        [ProtoMember(3)]
        public bool Jump;
        /// <summary>
        /// 转向
        /// </summary>
        [ProtoMember(4)]
        public float? NextAngle;
        /// <summary>
        /// 加速度
        /// </summary>
        [ProtoMember(5)]
        public float Acceleration;
        /// <summary>
        /// 当前速度
        /// </summary>
        [ProtoMember(6)]
        public float curSpeed;
        [ProtoMember(7)]
        public bool IsAttact;
        [ProtoMember(8)]
        public double ServerTime;
    }

    [ProtoContract(Name = "1007")]
    public class PerPawnCMDList
    {
        [ProtoMember(1)]
        public int instanceID;
        [ProtoMember(2/*,DataFormat = DataFormat.Default*/)]
        public List<InputCMD> transList = new List<InputCMD>();
    }

    [ProtoContract(Name = "1008")]
    public class CMDList
    {
        [ProtoMember(1)]
        public List<PerPawnCMDList> transList = new List<PerPawnCMDList>();
        [ProtoMember(2)]
        public double ServerTime;
    }
}
