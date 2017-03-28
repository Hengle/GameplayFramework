using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;


namespace ProtoBuf
{
    [ProtoContract(Name = "901")]
    public class ServerLogin
    {
        [ProtoMember(1)]
        public ServerType Type;
    }

    [ProtoContract(Name = "902")]
    public class ChildServerBeginWork
    {
        [ProtoMember(1)]
        public int Port;
    }
}
