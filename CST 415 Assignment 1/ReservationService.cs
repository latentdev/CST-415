using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CST_415_Assignment_1
{

    class ReservationService
    {
        int servicePort;
        Port[] ports;
        int keepAlive;
        Socket listeningSocket;
 
        public ReservationService(int servicePort = 30000, int startPort = 40000, int endPort = 40099, int keepAlive = 300)
        {
            this.servicePort = servicePort;
            ports = CreatePorts(startPort, endPort);
            this.keepAlive = keepAlive;
            listeningSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            Console.WriteLine("Socket created");
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, servicePort));
            Console.WriteLine("Socket bound to port " + servicePort.ToString());
        }

        public void Listen()
        {
            try
            {
                // receive a message from a client
                Console.WriteLine("Waiting for message from client...");
                byte[] buffer = new byte[255];
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                int result = listeningSocket.ReceiveFrom(buffer, ref remoteEP);

                Message msg = Message.FromPacket(buffer);
                Console.WriteLine("Received " + result.ToString() + " bytes: ");
                msg.ToString();
                Action(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception when receiving..." + ex.Message);
            }
        }

        private void Action(Message msg)
        {
            switch(msg.msg_type)
            {
                case (byte)msg_type.REQUEST_PORT:
                    {
                        RequestPort(msg);
                        break;
                    }
                case (byte)msg_type.LOOKUP_PORT:
                    {
                        LookUpPort(msg);
                        break;
                    }
                case (byte)msg_type.KEEP_ALIVE:
                    {
                        KeepAlive(msg);
                        break;
                    }
                case (byte)msg_type.CLOSE_PORT:
                    {
                        ClosePort(msg);
                        break;
                    }
                case (byte)msg_type.PORT_DEAD:
                    {
                        PortDead(msg);
                        break;
                    }
                case (byte)msg_type.STOP:
                    {
                        Stop(msg);
                        break;
                    }
                default:
                    break;
            }
        }

        private void RequestPort(Message msg)
        {
            //implement RequestPort();
            Console.WriteLine("Requesting Port: "+)
        }
        private void LookUpPort(Message msg)
        {
            //implement LookUpPort();
        }
        private void KeepAlive(Message msg)
        {
            //implement KeepAlive();
        }
        private void ClosePort(Message msg)
        {
            //implement ClosePort();
        }
        private void PortDead(Message msg)
        {
            //implement PortDead();
        }
        private void Stop(Message msg)
        {
            //implement Stop();
        }

        private Port[] CreatePorts(int startPort,int endPort)
        {
            Port[] ports = new Port[endPort - startPort+1];
            for(int i=0;i<ports.Length;i++)
            {
                ports[i] = new Port(startPort + i, (int)alive.DEAD);
            }
            return ports;
        }
    }
}
