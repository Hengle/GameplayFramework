using System;
using System.Collections.Generic;
using UnityEngine;
using Poi;
using System.Linq;

namespace Poi
{
    public class Target : ITarget
    {
        public ISkillTarget First => LockedTargets?.FirstOrDefault();
        public List<ISkillTarget> LockedTargets { get; internal set; }
        public Vector3 Point { get; set; }
    }
}