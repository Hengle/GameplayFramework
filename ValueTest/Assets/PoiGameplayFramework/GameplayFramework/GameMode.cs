using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class GameMode:ScriptableObject
    {
        public int AAA;

        [Tooltip("角色控制器")]
        public MonoBehaviour PlayerController;

        [Tooltip("默认角色")]
        public GameObject DefaultPawn;
    }
}
