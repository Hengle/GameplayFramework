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
            MMONet.MMOClient client = new MMONet.MMOClient();

            client.BeginConnect(IPAddress.Loopback, 40000, callback, client);
        }

        private void callback(IAsyncResult ar)
        {
            MMOClient c = ar.AsyncState as MMOClient;
            c.EndConnect(ar);

            
        }
    }
}
