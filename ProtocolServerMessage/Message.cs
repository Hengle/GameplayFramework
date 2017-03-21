using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;


namespace ProtoBuf
{
    public class Message
    {
    }

    [ProtoContract,ProtoID(2001)]
    public class ServerLogin
    {
        [ProtoMember(1)]
        public ServerType Type;
    }

    [ProtoContract, ProtoID(2002)]
    public class ChildServerBeginWork
    {
        [ProtoMember(1)]
        public int Port;
    }

    public enum ServerType
    {
        GlobalServer = 0,
        ChatServer = 1,
        
    }
}
