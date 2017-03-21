using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Poi;
using ProtoBuf;

namespace GlobalServer
{
    public class Server:MMONet.Server
    {
        public Process ChatServerProcess { get; private set; }
        public Process MyProcess { get; private set; }
        internal ChildServerClient ChatServer { get; set; }
        /// <summary>
        /// 成功链接但没有识别的Client
        /// </summary>
        public List<ChildServerClient> UnknownClient { get; private set; } = new List<ChildServerClient>();

        public void Run()
        {
            ProtoID.Init();
            ProtoID.Init("ProtocolServerMessage");

            MyProcess = Process.GetCurrentProcess();
            //MyProcess.EnableRaisingEvents = true;
            //MyProcess.Exited += MyProcess_Exited;
            //AppDomain.CurrentDomain.ProcessExit += MyProcess_Exited;

            InitChildServer();

            var time = new UtilTime();

            while (true)
            {
                time.Update();
                double delta = time.DeltaTime;

                for (int i = 0; i < UnknownClient.Count; i++)
                {
                    UnknownClient[i]?.Update(delta);
                }

                if (ChatServer!=null)
                {
                    ChatServer.Update(delta);
                }

            }
        }

        private void MyProcess_Exited(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 初始化子功能服务器
        /// </summary>
        private void InitChildServer()
        {
            ///开始监听
            IPEndPoint ipep = new IPEndPoint(IPAddress.Loopback, Port.ChildServerLogin);
            TcpListener listener = new TcpListener(ipep);
            listener.Start();
            listener.BeginAcceptSocket(ChildServerAcceptCallback, listener);



            ProcessStartInfo chatInfo = new ProcessStartInfo();
            chatInfo.FileName = "ChatServerProgram.exe";
#if DEBUG
            chatInfo.WindowStyle = ProcessWindowStyle.Minimized;
#else
            chatInfo.WindowStyle = ProcessWindowStyle.Hidden;
#endif

            ChatServerProcess = Process.Start(chatInfo);
        }

        private void ChildServerAcceptCallback(IAsyncResult ar)
        {
            TcpListener ls = ar.AsyncState as TcpListener;
            var socket = ls.EndAcceptSocket(ar);


            ChildServerClient client = new ChildServerClient(socket,this);
            client.BeginReceive();
            UnknownClient.Add(client);



            ls.BeginAcceptSocket(ChildServerAcceptCallback, ls);
        }

        public void Stop()
        {
            if (ChatServerProcess != null)
            {
                //ChatServer.Close();
                if (!ChatServerProcess.HasExited)
                {
                    ChatServerProcess.Kill();
                }
                
            }
        }
    }
}
