using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProtoBuf;

namespace MMONet
{
    public abstract class Client:IDisposable
    {
        /// <summary>
        /// 描述消息包长度字节所占的字节数
        /// </summary>
        public const int MsgDesLength = 2;
        /// <summary>
        /// 消息包类型ID 字节长度
        /// </summary>
        public const int MsgIDLength = ProtoIDAttribute.Length;

        /// <summary>
        /// 最大暂存消息个数
        /// </summary>
        public int MaxMsgCount { get; private set; } = 1024;

        public bool IsConnected { get; private set; } = false;
        /// <summary>
        /// 接受数组偏移量
        /// </summary>
        private int offset = 0;
        /// <summary>
        /// 接受缓冲区大小
        /// </summary>
        public int ReceiveBufferSize { get; private set; } = 16 * 1024;

        IList<ArraySegment<byte>> sendList = new List<ArraySegment<byte>>();
        IList<ArraySegment<byte>> waitList = new List<ArraySegment<byte>>();
        /// <summary>
        /// 发送使用的异步套接字操作
        /// </summary>
        SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs receiveEventArgs = new SocketAsyncEventArgs();

        public Client()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            InitEventArgs();
        }

        public void InitEventArgs()
        {
            lock (this)
            {
                if (!isInitEventArgs)
                {
                    sendEventArgs.Completed += Send_Completed;
                    receiveEventArgs.Completed += ReceiveEventArgs_Completed;
                    isInitEventArgs = true;
                }
            }
        }

        public Client(Socket socket)
        {
            Socket = socket;
            InitEventArgs();
        }

        public Socket Socket { get; protected set; }
        /// <summary>
        /// 当前是否在接受数据
        /// </summary>
        public bool IsReceive { get; private set; }

        public IAsyncResult BeginConnect(IPAddress ipAddress, int port, AsyncCallback callback, object state)
        {
            return Socket.BeginConnect(ipAddress, port, callback, state);
        }

        public void EndConnect(IAsyncResult ar)
        {
            Socket.EndConnect(ar);
            IsConnected = Socket.Connected;
        }

        #region Write

        public void Write<T>(T msg)
        {
            Write(msg, ProtoID.GetID<T>());
        }

        public void Write<T>(T msg, int msgID)
        {
            MemoryStream body = new MemoryStream();
            ProtoBuf.Serializer.Serialize(body, msg);
            Write(msgID, body);
        }

        /// <summary>
        /// 写入消息号和消息正文的序列化流，用于在不解析状态下转发。
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="body">流一定不能是反序列化后的</param>
        public void Write(int msgID, MemoryStream body)
        {
            MemoryStream sendmsg = new MemoryStream(MsgDesLength + MsgIDLength);
            ushort length = (ushort)body.Length;
            sendmsg.Write(BitConverter.GetBytes(length), 0, MsgDesLength);
            sendmsg.Write(BitConverter.GetBytes(msgID), 0, MsgIDLength);

            body.WriteTo(sendmsg);

            ///对齐流数据
            sendmsg.Seek(0, SeekOrigin.Begin);

            Write(sendmsg);
        }

        public void Write(MemoryStream sendmsg)
        {
            ArraySegment<byte> sendbytes = new ArraySegment<byte>(sendmsg.ToArray(), 0, (int)sendmsg.Length);
            Write(sendbytes);
        }

        public void Write(ArraySegment<byte> sendbytes)
        {
            lock (sendEventArgs)
            {
                if (sendList.Count == 0)
                {
                    sendList.Add(sendbytes);
                    Send();
                }
                else
                {
                    waitList.Add(sendbytes);
                }
            }
        }

        /// <summary>
        /// 套接字开始发送数据
        /// </summary>
        private void Send()
        {
            sendEventArgs.BufferList = sendList;

            if (!Socket.SendAsync(sendEventArgs))
            {
                if (sendEventArgs.SocketError == SocketError.Success)
                {
                    Send_Completed(Socket, sendEventArgs);
                }
            }
        }

        private void Send_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (sendEventArgs.SocketError == SocketError.Success)
            {
                lock (sendEventArgs)
                {
                    sendList = waitList;
                    waitList = new List<ArraySegment<byte>>();

                    if (sendList.Count > 0)
                    {
                        Send();
                    }
                }
            }
            else
            {
                throw new Exception();
            }
        }

        #endregion

        #region Read


        protected ReceiveFuncMode ReceiveMode { get; set; } = ReceiveFuncMode.UseAsyncEventArgs;

        public void BeginReceive()
        {
            IsReceive = true;
            offset = 0;
            byte[] buffer = new byte[ReceiveBufferSize];

            if (ReceiveMode == ReceiveFuncMode.UseAsyncEventArgs)
            {
                receiveEventArgs.SetBuffer(buffer, offset, ReceiveBufferSize - offset);
                if (!Socket.ReceiveAsync(receiveEventArgs))
                {
                    ReceiveEventArgs_Completed(Socket, receiveEventArgs);
                }
            }
            else
            {
                Socket.BeginReceive(buffer, offset, ReceiveBufferSize - offset,
                    SocketFlags.None, ReceiveCallback, buffer);
            }
        } 

        public void EndReceive()
        {
            IsReceive = false;
        }

        private void ReceiveEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (receiveEventArgs.SocketError == SocketError.Success)
            {
                int length = receiveEventArgs.BytesTransferred;
                if (length <= 0)
                {
                    ///ERROR
                    DisConnect();
                    return;
                }

                var buffer = receiveEventArgs.Buffer;
                ///现有数据长度 = 为本次接收长度 + 上次数据剩余长度
                length += offset;

                int readLength = 0;
                if (OnParse != null)
                {
                    readLength = OnParse(buffer,length);
                }
                else
                {
                    readLength = DefaultParse(buffer,length);
                }

                ///数据剩余长度 = 现有数据长度 - 已经读取长度
                offset = length - readLength;

                byte[] newbuffer = new byte[ReceiveBufferSize];
                ///将剩余数据拷贝到新的 接收缓存中
                if (offset > 0)
                {
                    Array.Copy(buffer, readLength, newbuffer, 0, offset);
                }

                if (IsReceive)
                {
                    receiveEventArgs.SetBuffer(newbuffer, offset, ReceiveBufferSize - offset);
                    Socket.ReceiveAsync(receiveEventArgs);
                }
            }
            else
            {
                throw new Exception();
            }
        }
         
        private void ReceiveCallback(IAsyncResult ar)
        {
            SocketError err;

            var buffer = ar.AsyncState as byte[];

            try
            {
                ///接收的数据长度
                int length = Socket.EndReceive(ar, out err);

                if (err == SocketError.SocketError || length <= 0)
                {
                    ///ERROR
                    DisConnect();
                    return;
                }

                ///现有数据长度 = 为本次接收长度 + 上次数据剩余长度
                length += offset;

                int readLength = 0;
                if (OnParse != null)
                {
                    readLength = OnParse(buffer, length);
                }
                else
                {
                    readLength = DefaultParse(buffer, length);
                }

                ///数据剩余长度 = 现有数据长度 - 已经读取长度
                offset = length - readLength;
                byte[] newbuffer = new byte[ReceiveBufferSize];
                ///将剩余数据拷贝到新的 接收缓存中
                if (offset > 0)
                {
                    Array.Copy(buffer, readLength, newbuffer, 0, offset);
                }
                if (IsReceive)
                {
                    Socket.BeginReceive(newbuffer, offset, ReceiveBufferSize - offset,
                        SocketFlags.None, ReceiveCallback, newbuffer);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 默认的解析方法，处理粘包
        /// </summary>
        /// <param name="buffer">接收数据包（可能大于有效数据长度）</param>
        /// <param name="length">有效数据长度</param>
        /// <returns>返回已经读取的整包的总长度</returns>
        private int DefaultParse(byte[] buffer, int length)
        {
            ///已经完整读取消息包的长度
            int tempoffset = 0;

            lock (this)
            {
                ///流长度至少要大于2（2个字节也就是一个消息包长度的描述）
                while ((length - tempoffset) > MsgDesLength)
                {
                    ///取得当前消息包正文的长度
                    ushort size = BitConverter.ToUInt16(buffer, tempoffset);

                    if (size > length - tempoffset - MsgDesLength - MsgIDLength)
                    {
                        ///剩余流长度没有完整包含一个消息包
                        break;
                    }

                    if (msgQueue.Count >= MaxMsgCount && KeepMode == KeepMsgMode.New)
                    {
                        ///消息过多直接舍弃
                        msgQueue.Dequeue();
                        DropMsgCount++;
                    }

                    if (msgQueue.Count < MaxMsgCount || KeepMode == KeepMsgMode.Old)
                    {
                        ///消息包格式依次为 消息包长度2 ，消息报类型2 ，消息正文，此处解析出消息类型
                        int msg_id = BitConverter.ToUInt16(buffer, tempoffset + MsgDesLength);

                        ///取得消息正文，起始偏移为tempoffset + 2 + 2；
                        MemoryStream msg = new MemoryStream(buffer, tempoffset + MsgDesLength + MsgIDLength, size, true, true);

                        msgQueue.Enqueue(new KeyValuePair<int, MemoryStream>(msg_id, msg));

                        //OnResponse(msg_id, msg); //此处为测试代码
                        //UpdateMessage(); //此处为测试代码
                    }

                    ///识别的消息包，移动流起始位置
                    tempoffset += (size + MsgDesLength + MsgIDLength);
                }
            }

            return tempoffset;
        }


        /// <summary>
        /// 上个处理消息轮询期间因为接收到消息溢出而舍弃的消息数量
        /// </summary>
        public int DropMsgCount { get; protected set; } = 0;
        /// <summary>
        /// 当接收到的消息大约消息缓冲池是如何保留消息
        /// </summary>
        protected KeepMsgMode KeepMode { get; set; } = KeepMsgMode.New;
        ///// <summary>
        ///// 默认的读取流方法，处理粘包
        ///// </summary>
        ///// <param name="transferred"></param>
        ///// <returns>返回已经读取的整包的总长度</returns>
        //private int DefaultParse(MemoryStream transferred)
        //{
        //    ///已经完整读取消息包的长度
        //    int tempoffset = 0;

        //    byte[] buffer = transferred.GetBuffer();

        //    lock (msgQueue)
        //    {
        //        ///流长度至少要大于2（2个字节也就是一个消息包长度的描述）
        //        while ((transferred.Length - tempoffset) > MsgDesLength)
        //        {
        //            ///取得当前消息包正文的长度
        //            ushort size = BitConverter.ToUInt16(buffer, tempoffset);

        //            if (size > transferred.Length - tempoffset - MsgDesLength - MsgIDLength)
        //            {
        //                ///剩余流长度没有完整包含一个消息包
        //                break;
        //            }

        //            if (msgQueue.Count >= MaxMsgCount && KeepMode == KeepMsgMode.New)
        //            {
        //                ///消息过多直接舍弃
        //                msgQueue.Dequeue();
        //                DropMsgCount++;
        //            }

        //            if (msgQueue.Count < MaxMsgCount || KeepMode == KeepMsgMode.Old)
        //            {
        //                ///消息包格式依次为 消息包长度2 ，消息报类型2 ，消息正文，此处解析出消息类型
        //                ushort msg_id = BitConverter.ToUInt16(buffer, tempoffset + MsgDesLength);

        //                ///取得消息正文，起始偏移为tempoffset + 2 + 2；
        //                MemoryStream msg = new MemoryStream(buffer, tempoffset + MsgDesLength + MsgIDLength, size, true, true);

        //                msgQueue.Enqueue(new KeyValuePair<ushort, MemoryStream>(msg_id, msg));
        //            }

        //            ///识别的消息包，移动流起始位置
        //            tempoffset += (size + MsgDesLength + MsgIDLength);
        //        }
        //    }

        //    transferred.Seek(tempoffset, SeekOrigin.Begin);

        //    return tempoffset;
        //}

        /// <summary>
        /// 数据解析方法
        /// </summary>
        public Func<byte[],int, int> OnParse;

        /// <summary>
        /// 已经读取到的消息
        /// </summary>
        protected Queue<KeyValuePair<int, MemoryStream>> msgQueue = new Queue<KeyValuePair<int, MemoryStream>>();
        protected Queue<KeyValuePair<int, MemoryStream>> msgQueue2 = new Queue<KeyValuePair<int, MemoryStream>>();
        protected Queue<KeyValuePair<int, MemoryStream>> dealMsgQueue = new Queue<KeyValuePair<int, MemoryStream>>();
        private bool isInitEventArgs = false;

        public virtual void DisConnect(DisConnectReason resason = DisConnectReason.Active)
        {
            if (IsReceive)
            {
                IsReceive = false;
            }

            if (IsConnected)
            {
                Socket.Disconnect(false);
            }

            OnDisConnect?.Invoke(this,resason);
        }

        public event Action<Client, DisConnectReason> OnDisConnect;

        /// <summary>
        /// 处理接收到的消息队列
        /// </summary>
        protected void UpdateMessage()
        {
            lock (msgQueue)
            {
                ///交换消息队列
                dealMsgQueue = msgQueue;
                msgQueue = msgQueue2;
                //var temp = msgQueue;
                //msgQueue = dealMsgQueue; //这种写法或导致Lock等待，原因待查
                //dealMsgQueue = msgQueue;
                //msgQueue = new Queue<KeyValuePair<ushort, MemoryStream>>(); //直接交互队列会丢失消息
                DropMsgCount = 0;
            }

            while (dealMsgQueue.Count > 0)
            {
                var msg = dealMsgQueue.Dequeue();
                OnResponse(msg.Key, msg.Value);
            }

            msgQueue2 = dealMsgQueue;
        }

        protected virtual void OnResponse(int key, MemoryStream value)
        {

        }

        public void Dispose()
        {
            if (IsReceive)
            {
                EndReceive();
            }

            if (IsConnected)
            {
                Socket.Disconnect(false);
            }

            (Socket as IDisposable).Dispose();
        }
        #endregion
    }


    public enum DisConnectReason
    {
        Error,
        /// <summary>
        /// 主动断开
        /// </summary>
        Active,
        /// <summary>
        /// 远端断开
        /// </summary>
        Remote,
    }


    public enum KeepMsgMode
    {
        /// <summary>
        /// 保留最新收到的消息
        /// </summary>
        New,
        /// <summary>
        /// 保留之前收到的消息
        /// </summary>
        Old,
    }

    /// <summary>
    /// 结束数据模式
    /// </summary>
    public enum ReceiveFuncMode
    {
        UseAsyncEventArgs,
        BeginReceive,
    }
}
