using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSocket
{
    class Program
    {
        private static Socket _socket;
        private static object ipEndPoint;
        private static string input;

        static void Main(string[] args)
        {
            Console.WriteLine("----- socket client ------\n");
            Console.Write("Adresse IP: ");
            string adress = Console.ReadLine();
            Console.Write("Port : ");
            string port = Console.ReadLine();

            ClientTCP(IPAddress.Parse(adress), int.Parse(port));

        }

        private static void ClientTCP(IPAddress adress, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(adress, port);

            _socket.Connect(ipEndPoint);
            var thread = new Thread(Listen);
            thread.Start();
        }

        private static void Listen()
        {
            while (true)
            {
                input = Console.ReadLine();
                _socket.Send(Encoding.UTF8.GetBytes(input));

                var buffer = new byte[_socket.SendBufferSize];
                var readBytes = _socket.Receive(buffer);
                if (readBytes > 0)
                {
                    var msg = Encoding.UTF8.GetString(buffer, 0, readBytes);
                    Console.WriteLine(msg);
                }
            }
        }
    }
}

