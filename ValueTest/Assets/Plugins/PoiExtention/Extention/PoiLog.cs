using System;
using System.Diagnostics;

namespace UnityEngine
{
    public class PoiLog
    {
        [Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }
    }
}