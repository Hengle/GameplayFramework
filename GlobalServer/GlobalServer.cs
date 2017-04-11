using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        public UtilTime Time { get; private set; }

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


            Time = new UtilTime();

            while (true)
            {
                Time.Update();
                double delta = Time.DeltaTime;

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
                    ///同一时间间隔中位置只有一个，而收到的指令可能有多个

                    ///此处在客户端增多是需要做分包处理Todo

                    ///同步位置
                    TransList msg = new TransList()
                    {
                        ServerTime = Time.TotalMilliseconds,
                    };

                    ///同步操作指令
                    CMDList cmdMsg = new CMDList()
                    {
                        ServerTime = Time.TotalMilliseconds,
                    };

                    foreach (var item in GameClient.ClientDic)
                    {
                        GameClient client = item.Value as GameClient;
                        client.Update(delta);
     
                        if (client.SyncTransCD.Check(delta,true))
                        {
                            msg.transList.Add(new TransSync()
                            {
                                instanceID = client.InstanceID,
                                trans = client.Trans
                            });
                        }

                        ///客户端同步操作命令
                        if (client.CmdList.Count > 0)
                        {
                            cmdMsg.transList.Add(new PerPawnCMDList()
                            {
                                instanceID = client.InstanceID,
                                transList = new List<InputCMD>(client.CmdList),
                            });
                            client.CmdList.Clear();
                        }    
                    }

                    ///存在客户端
                    if (GameClient.ClientDic.Count > 0)
                    {
                        if (msg.transList.Count > 0)
                        {
                            GameClient.BroadCast(msg);
                        }

                        if (cmdMsg.transList.Count > 0)
                        {
                            GameClient.BroadCast(cmdMsg);
                        }
                    }

                    lock (GameClient.AddRemoveList)
                    {
                        while (GameClient.AddRemoveList.Count > 0)
                        {
                            var item = GameClient.AddRemoveList.Dequeue();
                            GameClient client = item.Value as GameClient;
                            if (item.Key == MMONet.Remote4Server.AddRemove.Add)
                            {
                                GameClient.ClientDic.Add(client.InstanceID, client);
                                Console.WriteLine($"客户端{client.Account}登陆，分配临时ID：{client.InstanceID}。" +
                                    $"当前客户端数量：{GameClient.ClientDic.Count}。");
                            }
                            else
                            {
                                GameClient.ClientDic.Remove(item.Value.InstanceID);
                                Console.WriteLine($"客户端{client.Account}退出，ID：{client.InstanceID}。" +
                                    $"当前客户端数量：{GameClient.ClientDic.Count}。");
                            }
                        }
                    }
                }

                Thread.Sleep(20);
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
