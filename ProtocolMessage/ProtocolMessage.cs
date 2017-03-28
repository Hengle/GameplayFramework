﻿using System;
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
}
