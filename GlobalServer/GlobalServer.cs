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
        public List<MMONet.Remote> UnknownClient { get; private set; } = new List<MMONet.Remote>();

        public void Run()
        {
            ProtoID.Init();
            ProtoID.Init("ProtocolServerMessage");

            MyProcess = Process.GetCurrentProcess();
            //MyProcess.EnableRaisingEvents = true;
            //MyProcess.Exited += MyProcess_Exited;
            //AppDomain.CurrentDomain.ProcessExit += new EventHandler(MyProcess_Exited);

            InitWork();

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

                if (ChatServer != null)
                {
                    ChatServer.Update(delta);
                }

                ///轮询客户端
                lock (GameClient.clientDic)
                {
                    foreach (var item in GameClient.clientDic)
                    {
                        item.Value.Update(delta);
                    }
                }
            }
        }

        private void InitWork()
        {

            ///开始监听客户端连接
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Port.GlobalListen);
            TcpListener listener = new TcpListener(ipep);
            listener.Start();
            listener.BeginAcceptSocket(ClientAcceptCallback, listener);

            Console.WriteLine($"开始监听客户端，监听端口：{Port.GlobalListen}");
        }

        private void ClientAcceptCallback(IAsyncResult ar)
        {
            TcpListener ls = ar.AsyncState as TcpListener;
            var socket = ls.EndAcceptSocket(ar);
            ls.BeginAcceptSocket(ClientAcceptCallback, ls);
            GameClient client = new GameClient(socket,this);
            if (client.IsConnected)
            {
                UnknownClient.Add(client);
                client.BeginReceive();
            }
        }

        private void MyProcess_Exited(object sender, EventArgs e)
        {
            Stop();
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
            Console.WriteLine($"初始化子服务器，监听端口：{Port.ChildServerLogin}");


            var p = Process.GetProcessesByName("ChatServerProgram");
            foreach (var item in p)
            {
                item.Kill();
            }
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
