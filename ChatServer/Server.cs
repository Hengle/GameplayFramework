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
    public class Server:MMOServer
    {
        public int ListenPort { get; private set; } = 40000;
        List<Client> clientList = new List<Client>();


        public void Run()
        {
            ProtoID.Init();

            ///开始监听
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, ListenPort);
            TcpListener listener = new TcpListener(ipep);
            listener.Start();
            listener.BeginAcceptSocket(AcceptCallback, listener);

            var time = new UtilTime();

            while (true)
            {
                time.Update();
                double delta = time.DeltaTime;

                while (accepedSocket.Count > 0)
                {
                    Client client = new Client(accepedSocket.Dequeue());
              
                }

                foreach (var item in clientList)
                {
                    item.Update(delta);
                }

                Thread.Sleep(0);
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
