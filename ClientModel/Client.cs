using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Protocol;
using System.Text;
using System.Threading.Tasks;

namespace ClientModel
{
    public class Client
    {
        string ADDRESS = "127.0.0.1";
        int PORT = 30000;
        Socket clientSocket;
        IPEndPoint endPt;
        public Message response { get; set; }

        public event EventHandler<EventArgs> DataReceived;

        public Client()
        {
            // create the socket for sending messages to the server
            clientSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            Console.WriteLine("Socket created");

            // construct the server's address and port
            endPt = new IPEndPoint(IPAddress.Parse(ADDRESS), PORT);
        }

        public void RequestPort()
        {
            try
            {
                Message msg = new Message((byte)msg_type.REQUEST_PORT, "test_client".ToCharArray(), 40064, (byte)status.SUCCESS);

                byte[] buffer = msg.ToPacket();
                int result = clientSocket.SendTo(buffer, endPt);

                buffer = new byte[255];
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                result = clientSocket.ReceiveFrom(buffer, ref remoteEP);
                response = Message.FromPacket(buffer);
                DataReceived?.Invoke(this, new EventArgs());
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void LookUpPort(UInt16 port)
        {
            try
            {
                Message msg = new Message((byte)msg_type.LOOKUP_PORT, "test_client".ToCharArray(), port, (byte)status.SUCCESS);

                byte[] buffer = msg.ToPacket();
                int result = clientSocket.SendTo(buffer, endPt);

                buffer = new byte[255];
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                result = clientSocket.ReceiveFrom(buffer, ref remoteEP);
                response = Message.FromPacket(buffer);
                DataReceived?.Invoke(this, new EventArgs());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
