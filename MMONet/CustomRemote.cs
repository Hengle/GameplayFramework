using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MMONet
{
    /// <summary>
    /// 通用性远端
    /// </summary>
    public class CustomRemote : Remote
    {
        protected override void Response(int key, MemoryStream value)
        {
            OnResponse?.Invoke(key, value);
        }

        public event Action<int, MemoryStream> OnResponse;
    }

    /// <summary>
    /// 通用性远端
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomRemote<T>:CustomRemote
    {
        
    }
}
