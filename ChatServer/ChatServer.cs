using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using MMONet;
using Poi;
using ProtoBuf;

namespace ChatServer
{
    public class Server: MMONet.Server
    {
        internal GlobalServerClient GServer { get; private set; }

        public List<Client> UnknownClientList { get; private set; } = new List<Client>();

        public void Run()
        {
            ///连接全局服务器
            GlobalServerClient server = new GlobalServerClient(this);
            server.BeginConnect(IPAddress.Loopback, Port.ChildServerLogin,
                GlobalServerConnectCallback, server);



            var time = new UtilTime();

            while (true)
            {

                time.Update();
                double delta = time.DeltaTime;

                if (GServer != null)
                {
                    GServer.Update(delta);
                }

                while (accepedSocket.Count > 0)
                {
                    Client client = new Client(accepedSocket.Dequeue(), this);
                    UnknownClientList.Add(client);
                    client.OnDisConnect += (cl, res) =>
                    {
                        lock (UnknownClientList)
                        {
                            UnknownClientList.Remove(cl as Client);
                            cl.Dispose();
                        }
                    };
                }

                for (int i = 0; i < UnknownClientList.Count; i++)
                {
                    UnknownClientList[i].Update(delta);
                }

                lock (Client.ClientDic)
                {
                    foreach (var item in Client.ClientDic)
                    {
                        item.Value.Update(delta);
                    }

                    lock (Client.AddRemoveList)
                    {
                        while (Client.AddRemoveList.Count > 0)
                        {
                            var item = Client.AddRemoveList.Dequeue();
                            Client client = item.Value as Client;
                            if (item.Key == MMONet.Remote4Server.AddRemove.Add)
                            {
                                Client.ClientDic.Add(client.InstanceID, client);
                                Console.WriteLine($"客户端{client.InstanceID}登陆。当前客户端数量：{Client.ClientDic.Count}。");
                            }
                            else
                            {
                                Client.ClientDic.Remove(item.Value.InstanceID);
                                Console.WriteLine($"客户端{client.InstanceID}退出。当前客户端数量：{Client.ClientDic.Count}。");
                            }
                        }
                    }
                }
                Thread.Sleep(20);

            }
        }

        internal void BeginWork(int port)
        {
            ///开始监听
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            TcpListener listener = new TcpListener(ipep);
            listener.Start();
            listener.BeginAcceptSocket(AcceptCallback, listener);

            Console.WriteLine($"开始监听:{port}端口");
        }

        private void GlobalServerConnectCallback(IAsyncResult ar)
        {
            GlobalServerClient server = ar.AsyncState as GlobalServerClient;
            server.EndConnect(ar);
            if (server.IsConnected)
            {
                GServer = server;
                GServer.BeginReceive();

                var msg = new ServerLogin();
                msg.Type = ServerType.ChatServer;
                GServer.Write(msg);
            }
            else
            {

            }    
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                var ls = ar.AsyncState as TcpListener;
                Socket accept = ls.EndAcceptSocket(ar);
                ///加入队列
                accepedSocket.Enqueue(accept);
                ls.BeginAcceptSocket(AcceptCallback, ls);
            }
            catch (Exception)
            {

                throw;
            }
        }


        Queue<Socket> accepedSocket = new Queue<Socket>();
        
    }
}
