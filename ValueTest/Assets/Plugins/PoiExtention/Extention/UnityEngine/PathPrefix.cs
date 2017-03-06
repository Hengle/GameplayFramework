using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 路径前缀
    /// </summary>
    public static class PathPrefix
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        /// <summary>
        /// WWW加载资源的前缀
        /// </summary>
        public static readonly string WWWstreamingAssets = "file:///";
#elif UNITY_ANDROID
        public static readonly string WWWstreamingAssets ="";// "jar:file:///";
#endif
        public static readonly string WWWpersistentData = "file:///";
    }
}
