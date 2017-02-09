using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public abstract class Target
    {
        public Vector3 TargetWorldPosotion { get; set; } = Vector3.zero;
    }

    public class TransformTarget : Target
    {
        public new Vector3 TargetWorldPosotion => Target == null ? Vector3.zero : Target.position;

        public Transform Target { get; set; }
    }

    public class PointTarget : Target
    {

    }
}
