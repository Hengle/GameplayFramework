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
        public LineMode LineMode { get; private set; } = LineMode.Online;
        internal ChildServerClient ChatServer { get; set; }
        /// <summary>
        /// 成功链接但没有识别的Client
        /// </summary>
        public List<MMONet.Remote> UnknownClient { get; private set; } = new List<MMONet.Remote>();
        public IPAddress CurrentIP { get; private set; } = null;

        public void Init(string[] args)
        {
            try
            {
                foreach (var item in args)
                {
                    if (item.StartsWith("--LineMode"))
                    {
                        string value = item.Split('=')[1];
                        LineMode = value.ToEnum<LineMode>();
                        
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                CurrentIP = IPAddressExtention.GetIP(LineMode == LineMode.LAN);
                PDebug.Log(CurrentIP);
            } 
        }

        public void Run()
        {
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
                lock (GameClient.ClientDic)
                {
                    TransList msg = new TransList();
                    foreach (var item in GameClient.ClientDic)
                    {
                        GameClient client = item.Value as GameClient;
                        client.Update(delta);
                        msg.transList.Add(new TransSync()
                        {
                            instanceID = client.InstanceID,
                            trans = client.Trans
                        });
                    }
                    if (msg.transList.Count > 0 && GameClient.ClientDic.Count > 0)
                    {
                        GameClient.BroadCast(msg);
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

                PDebug.Log("新客户端link");
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
