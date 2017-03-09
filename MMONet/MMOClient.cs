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
    public class MMOClient
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
        /// 接受数组偏移量
        /// </summary>
        protected int offset = 0;
        /// <summary>
        /// 接受缓冲区大小
        /// </summary>
        public int ReceiveBufferSize { get; private set; } = 8092;

        private IPAddress loopback;
        private int v;

        public MMOClient()
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
                    isInitEventArgs = true;
                }
            }
        }

        public MMOClient(Socket socket)
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
        }

        #region Write



        public void Write<T>(T msg)
        {
            Write(msg, ProtoID.GetID<T>());
        }

        public void Write<T>(T msg, ushort msgID)
        {
            MemoryStream body = new MemoryStream();
            ProtoBuf.Serializer.Serialize(body, msg);
            MemoryStream sendmsg = new MemoryStream(MsgDesLength + MsgIDLength);
            ushort length = (ushort)body.Length;
            sendmsg.Write(BitConverter.GetBytes(length), 0, MsgDesLength);
            sendmsg.Write(BitConverter.GetBytes(msgID), 0, MsgIDLength);

            body.WriteTo(sendmsg);

            ///对齐流数据
            sendmsg.Seek(0, SeekOrigin.Begin);

            Write(sendmsg);
        }

        IList<ArraySegment<byte>> sendList = new List<ArraySegment<byte>>();
        IList<ArraySegment<byte>> waitList = new List<ArraySegment<byte>>();
        SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();

        public void Write(MemoryStream sendmsg)
        {
            ArraySegment<byte> sendbytes = new ArraySegment<byte>(sendmsg.ToArray(), 0, (int)sendmsg.Length);
            Write(sendbytes);
        }

        public void Write(ArraySegment<byte> sendbytes)
        {
            lock (sendList)
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
            if (e.SocketError == SocketError.Success)
            {
                lock (sendList)
                {
                    sendList.Clear();
                    var temp = sendList;
                    sendList = waitList;
                    waitList = temp;

                    if (sendList.Count > 0)
                    {
                        Send();
                    }
                }
            }
        }

        #endregion

        #region Read

        public void BeginReceive()
        {
            IsReceive = true;
            byte[] buffer = new byte[ReceiveBufferSize];
            Socket.BeginReceive(buffer, offset, ReceiveBufferSize, SocketFlags.None, ReceiveCallback, buffer);
        }

        public void EndReceive()
        {
            IsReceive = false;
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
                    Disconnect();
                }

                ///现有数据长度 = 为本次接收长度 + 上次数据剩余长度
                length += offset;

                MemoryStream transferred = new MemoryStream(buffer, 0, length, true, true);

                int readLength = 0;
                if (OnRead != null)
                {
                    readLength = OnRead(transferred);
                }
                else
                {
                    readLength = DefaultOnRead(transferred);
                }

                ///数据剩余长度 = 现有数据长度 - 已经读取长度
                offset = length - readLength;

                byte[] newbuffer = new byte[ReceiveBufferSize];
                ///将剩余数据拷贝到新的 接收缓存中 (readLength + 1?)
                Array.Copy(buffer, readLength, newbuffer, 0, offset);
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
        /// 默认的读取流方法，处理粘包
        /// </summary>
        /// <param name="transferred"></param>
        /// <returns>返回已经读取的整包的总长度</returns>
        private int DefaultOnRead(MemoryStream transferred)
        {
            ///已经完整读取消息包的长度
            int tempoffset = 0;

            byte[] buffer = transferred.GetBuffer();

            lock (this)
            {
                ///流长度至少要大于2（2个字节也就是一个消息包长度的描述）
                while ((transferred.Length - tempoffset) > MsgDesLength)
                {
                    ///取得当前消息包正文的长度
                    ushort size = BitConverter.ToUInt16(buffer, tempoffset);

                    if (size > transferred.Length - tempoffset - MsgDesLength - MsgIDLength)
                    {
                        ///剩余流长度没有完整包含一个消息包
                        break;
                    }

                    ///消息包格式依次为 消息包长度2 ，消息报类型2 ，消息正文，此处解析出消息类型
                    ushort msg_id = BitConverter.ToUInt16(buffer, tempoffset + MsgDesLength);

                    ///取得消息正文，起始偏移为tempoffset + 2 + 2；
                    MemoryStream msg = new MemoryStream(buffer, tempoffset + MsgDesLength + MsgIDLength, size, true, true);
                    msgQueue.Enqueue(new KeyValuePair<ushort, MemoryStream>(msg_id, msg));

                    tempoffset += (size + MsgDesLength + MsgIDLength);
                }
            }

            transferred.Seek(tempoffset, SeekOrigin.Begin);

            return tempoffset;
        }

        /// <summary>
        /// 流读取方法
        /// </summary>
        public Func<MemoryStream, int> OnRead;

        /// <summary>
        /// 已经读取到的消息
        /// </summary>
        protected Queue<KeyValuePair<ushort, MemoryStream>> msgQueue = new Queue<KeyValuePair<ushort, MemoryStream>>();
        protected Queue<KeyValuePair<ushort, MemoryStream>> dealMsgQueue = new Queue<KeyValuePair<ushort, MemoryStream>>();
        private bool isInitEventArgs = false;

        private void Disconnect()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 处理接收到的消息队列
        /// </summary>
        protected void UpdateMessage()
        {
            lock (msgQueue)
            {
                ///交换消息队列
                var temp = msgQueue;
                msgQueue = dealMsgQueue;
                dealMsgQueue = msgQueue;
            }

            while (dealMsgQueue.Count > 0)
            {
                var msg = dealMsgQueue.Dequeue();
                OnResponse(msg.Key, msg.Value);
            }
        }

        protected virtual void OnResponse(ushort key, MemoryStream value)
        {

        }
        #endregion
    }
}
