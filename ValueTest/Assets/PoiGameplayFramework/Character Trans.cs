using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace Poi
{
    public partial class Character
    {
        public void Move(Trans trans)
        {
            transform.position = new Vector3(trans.x, trans.y, trans.z);
            transform.rotation = new Quaternion(trans.qx, trans.qy, trans.qz, trans.qw);
        }
    }
}
