using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CST_415_Assignment_1_Client
{
    class Client
    {
        string ADDRESS = "127.0.0.1";
        int PORT = 30000;
        Socket clientSocket;
        IPEndPoint endPt;

        public Client()
        {
            // create the socket for sending messages to the server
            clientSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            Console.WriteLine("Socket created");

            // construct the server's address and port
            endPt = new IPEndPoint(IPAddress.Parse(ADDRESS), PORT);
        }
        public void Loop()
        {
            try
            {
                // send a message to the server
                Console.WriteLine("Sending message to server...");
                Message msg = new Message((byte)msg_type.REQUEST_PORT, "test_service".ToCharArray(), 40064, (byte)status.SUCCESS);

                byte[] buffer = msg.ToPacket();
                int result = clientSocket.SendTo(buffer, endPt);
                Console.WriteLine("Sent " + result.ToString() + " bytes");

                // receive a message from the server
                Console.WriteLine("Waiting for message from server...");
                buffer = new byte[54];
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                result = clientSocket.ReceiveFrom(buffer, ref remoteEP);
                Console.WriteLine("Received " + result.ToString() + " bytes: " + new string(ASCIIEncoding.UTF8.GetChars(buffer)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception when receiving..." + ex.Message);
            }

            // close the socket and quit
            Console.WriteLine("Closing down");
            clientSocket.Close();
            Console.WriteLine("Closed!");

            Console.ReadKey();
        }
    }
}
