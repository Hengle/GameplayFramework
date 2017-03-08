using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using MMONet;
using ProtoBuf;

namespace ChatServer
{
    internal class Client : MMOClient
    {
        /// <summary>
        /// 描述消息包长度字节所占的字节数
        /// </summary>
        private const int MsgDesLength = 2;
        /// <summary>
        /// 消息包类型ID 字节长度
        /// </summary>
        private const int MsgIDLength = ProtoIDAttribute.Length;
        /// <summary>
        /// 接受数组偏移量
        /// </summary>
        private int offset = 0;
        private Socket socket;

        public Client(Socket socket)
        {
            this.socket = socket;
            byte[] buffer = new byte[ReceiveBufferSize];
            socket.BeginReceive(buffer, offset, ReceiveBufferSize, SocketFlags.None, ReceiveCallback, buffer);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            SocketError err;

            var buffer = ar.AsyncState as byte[];

            try
            {
                ///接收的数据长度
                int length = socket.EndReceive(ar, out err);

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

                socket.BeginReceive(newbuffer, offset, ReceiveBufferSize - offset, SocketFlags.None, ReceiveCallback, newbuffer);
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
        Queue<KeyValuePair<ushort, MemoryStream>> msgQueue = new Queue<KeyValuePair<ushort, MemoryStream>>();
        Queue<KeyValuePair<ushort, MemoryStream>> dealMsgQueue = new Queue<KeyValuePair<ushort, MemoryStream>>();
        private void Disconnect()
        {
            throw new NotImplementedException();
        }

        public int ReceiveBufferSize { get; private set; } = 8092;

        internal void Update(double deltaTime)
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


        private void OnResponse(ushort key, MemoryStream value)
        {
            if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
            else if (key == ProtoID.GetID<ChatMsg>()) OnChatMsg(value);
        }

        private void OnChatMsg(MemoryStream value)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(T msg)   
        {
            Write(msg, ProtoID.GetID<T>());
        }

        public void Write<T>(T msg,ushort msgID)
        {
            MemoryStream body = new MemoryStream();
            Serializer.Serialize(body, msg);
            MemoryStream header = new MemoryStream(MsgDesLength + MsgIDLength);
            ushort length = (ushort)body.Length;
            header.Write(BitConverter.GetBytes(length), 0, MsgDesLength);
            header.Write(BitConverter.GetBytes(msgID), 0, MsgIDLength);

            body.WriteTo(header);

            ///对齐流数据
            header.Seek(0, SeekOrigin.Begin);

            Write(header);
        }

        public void Write(MemoryStream header)
        {
            
        }
    }
}