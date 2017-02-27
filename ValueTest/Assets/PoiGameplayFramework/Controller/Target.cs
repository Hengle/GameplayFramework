using System;
using System.Collections.Generic;
using UnityEngine;
using Poi;
using System.Linq;

namespace Poi
{
    public class Target : ITarget
    {
        public Transform First => LockedTargets?.FirstOrDefault();
        public List<Transform> LockedTargets { get; internal set; }
        public Vector3 Point { get; set; }
    }
}