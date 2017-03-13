using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MMONet;

namespace ChatServer
{
    public class ClientTempTest
    {
        public void Test()
        {
            MMONet.Client client = null;// new MMONet.Client();

            client.BeginConnect(IPAddress.Loopback, 40000, callback, client);
        }

        private void callback(IAsyncResult ar)
        {
            MMONet.Client c = ar.AsyncState as MMONet.Client;
            c.EndConnect(ar);

            
        }
    }
}
