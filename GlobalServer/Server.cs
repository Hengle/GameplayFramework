using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poi;
using ProtoBuf;

namespace GlobalServer
{
    public class Server:MMONet.Server
    {
        public Process ChatServer { get; private set; }

        public void Run()
        {
            ProtoID.Init();
            ProtoID.Init("ProtocolServerMessage");

            var myProcess = Process.GetCurrentProcess();
            myProcess.Exited += MyProcess_Exited;

            ProcessStartInfo chatInfo = new ProcessStartInfo();
            chatInfo.FileName = "ChatServerProgram.exe";
#if DEBUG
            chatInfo.WindowStyle = ProcessWindowStyle.Minimized;
#else
            chatInfo.WindowStyle = ProcessWindowStyle.Hidden;
#endif

            ChatServer = Process.Start(chatInfo);
        }

        private void MyProcess_Exited(object sender, EventArgs e)
        {
            if (ChatServer != null)
            {
                ChatServer.Close();
            }
        }
    }
}
