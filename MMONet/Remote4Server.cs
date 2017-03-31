using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace MMONet
{
    /// <summary>
    /// 服务器使用的远端，自身含有静态的集合，含有一个临时ID，含有广播方法
    /// </summary>
    public abstract class Remote4Server:Remote
    {
        public static Dictionary<int, Remote4Server> ClientDic { get; protected set; }
                                                    = new Dictionary<int, Remote4Server>();
        public int InstanceID { get; protected set; }
        public static Queue<KeyValuePair<AddRemove, Remote4Server>> AddRemoveList { get => addRemoveList;private set => addRemoveList = value; }

        public Remote4Server(Socket socket) : base(socket)
        {
        }

        /// <summary>
        /// 向所有客户端广播消息,不包含自身
        /// </summary>
        /// <param name="msgbody">消息实例</param>
        public void BroadCastExceptSelf<T>(T msg)
        {
             BroadCast(CreateMsgbyte(msg), new int[] { InstanceID });
        }

        /// <summary>
        /// 向所有客户端广播消息,不包含自身
        /// <para>用于接到消息流，在为解析状态下直接广播</para>
        /// </summary>
        /// <param name="key">消息ID</param>
        /// <param name="msgbody">消息实例序列化流</param>
        public void BroadCastExceptSelf(int key, MemoryStream msgbody)
        {
             BroadCast(key, msgbody, new int[] { InstanceID });
        }



        public static void BroadCast<T>(T msg)
        {
            BroadCast(CreateMsgbyte(msg), null);
        }

        /// <summary>
        /// 向客户端广播消息
        /// <para>用于接到消息流，在为解析状态下直接广播</para>
        /// </summary>
        /// <param name="key">消息ID</param>
        /// <param name="msgbody">消息实例序列化流</param>
        /// <param name="exceptClient">排除的指定客户端</param>
        public static void BroadCast(int key, MemoryStream msgbody, IList<int> exceptClient)
        {
            var msg = CombineIDMsg(key, msgbody);
            ArraySegment<byte> sendbytes = new ArraySegment<byte>(msg.ToArray(), 0, (int)msg.Length);
            BroadCast(sendbytes, exceptClient);
        }

        protected static void BroadCast(ArraySegment<byte> sendbytes, IList<int> exceptClient)
        {
            lock (ClientDic)
            {
                foreach (var item in ClientDic)
                {
                    if (exceptClient != null && exceptClient.Contains(item.Key))
                    {
                        continue;
                    }
                    item.Value.Write(sendbytes);
                }
            }  
        }

        static Queue<KeyValuePair<AddRemove, Remote4Server>> addRemoveList = new Queue<KeyValuePair<AddRemove, Remote4Server>>();
        public static void RemoveRemote(Remote4Server client)
        {
            AddRemoveList.Enqueue(new KeyValuePair<AddRemove, Remote4Server>( AddRemove.Remove,client));
        }

        public static void AddRemote(Remote4Server client)
        {
            AddRemoveList.Enqueue(new KeyValuePair<AddRemove, Remote4Server>(AddRemove.Add, client));
        }

        public enum AddRemove
        {
            Add,
            Remove,
        }
    }
}