using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServeurSocket
{
    class Program
    {
        private static Socket _listenerSocket;
        private static Socket socketReception;
        private static byte[] buffer;
        private static int readBytes;

        static void Main(string[] args)
        {
            Console.WriteLine("----- socket serveur ------\n");
            Console.Write("Port : ");
            string port = Console.ReadLine();
            ServerTCP(int.Parse(port));
        }

        private static void ServerTCP(int port)
        {
            _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var address = GetHostIPAddress();
            IPEndPoint ip = new IPEndPoint(address, port);
            _listenerSocket.Bind(ip);

            var listenThread = new Thread(ListenThread);
            listenThread.Start();
            Console.WriteLine($"Success... Listening TCP: {address}:{port}");
        }

        private static void ListenThread()
        {
            while (true)
            {
                _listenerSocket.Listen(0);
                //Clients.Add(new ClientManager(  ));
                socketReception = _listenerSocket.Accept();
                buffer = new byte[socketReception.SendBufferSize];
                readBytes = socketReception.Receive(buffer);
                if (readBytes > 0)
                {
                    var msg = Encoding.UTF8.GetString(buffer, 0, readBytes);
                    Console.WriteLine(msg);
                }
            }
        }

        private static IPAddress GetHostIPAddress()
        {

            return Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork);

        }
        

    }
}
