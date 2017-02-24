using System;
using UnityEngine;

namespace Poi
{
    public class Target : ITarget
    {
        public Transform First { get; set; }

        public Vector3 Point { get; set; }
    }
}