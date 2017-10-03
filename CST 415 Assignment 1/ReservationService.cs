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

                Console.WriteLine();

                Message msg = Message.FromPacket(buffer);
                Console.WriteLine("Received " + result.ToString() + " bytes ");
                Console.WriteLine(msg.ToString());
                Console.WriteLine();

                msg = Action(msg);
                Console.WriteLine("Sending message to client...");
                result = listeningSocket.SendTo(msg.ToPacket(), remoteEP);
                Console.WriteLine("Sent " + result.ToString() + " bytes");
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception when receiving..." + ex.Message);
            }
        }

        private Message Action(Message in_msg)
        {
            Message msg;
            switch(in_msg.msg_type)
            {
                case (byte)msg_type.REQUEST_PORT:
                    {
                        msg = RequestPort(in_msg);
                        break;
                    }
                case (byte)msg_type.LOOKUP_PORT:
                    {
                        msg = LookUpPort(in_msg);
                        break;
                    }
                case (byte)msg_type.KEEP_ALIVE:
                    {
                        msg = KeepAlive(in_msg);
                        break;
                    }
                case (byte)msg_type.CLOSE_PORT:
                    {
                        msg = ClosePort(in_msg);
                        break;
                    }
                case (byte)msg_type.PORT_DEAD:
                    {
                        msg = PortDead(in_msg);
                        break;
                    }
                case (byte)msg_type.STOP:
                    {
                        msg = Stop(in_msg);
                        break;
                    }
                default:
                    {
                        msg = new Message((byte)msg_type.RESPONSE, in_msg.service_name, in_msg.port, (byte)status.INVALID_ARG);
                        break;
                    }
            }
            return msg;
        }

        private Message RequestPort(Message msg)
        {
            //implement RequestPort();
            Console.WriteLine("Service: " + new string(msg.service_name));
            Console.WriteLine("Requesting Port");
            return LowestPort(msg);
        }
        private Message LookUpPort(Message msg)
        {
            //implement LookUpPort();
            Console.WriteLine("Looking Up Port for Service: " + new string(msg.service_name));
            return FindService(msg);
        }
        private Message KeepAlive(Message msg)
        {
            //implement KeepAlive();
            Console.WriteLine("Service: " + new string(msg.service_name));
            Console.WriteLine("Recieved Keep Alive for Port: " + msg.port);
            return msg;
        }
        private Message ClosePort(Message msg)
        {
            //implement ClosePort();
            Console.WriteLine("Service: " + new string(msg.service_name));
            Console.WriteLine("Closing Port: " + msg.port);
            return msg;
        }
        private Message PortDead(Message msg)
        {
            //implement PortDead();
            Console.WriteLine("Service: " + new string(msg.service_name));
            Console.WriteLine("Marking Port " + msg.port + " Dead");
            return msg;
        }
        private Message Stop(Message msg)
        {
            //implement Stop();
            Console.WriteLine("Service: " + new string(msg.service_name));
            Console.WriteLine("Stopping Server");
            return msg;
        }

        private Port[] CreatePorts(int startPort,int endPort)
        {
            Port[] ports = new Port[endPort - startPort+1];
            for(int i=0;i<ports.Length;i++)
            {
                ports[i] = new Port((UInt16)(startPort + i), (int)alive.DEAD);
            }
            return ports;
        }
        private Message LowestPort(Message in_msg)
        {
            Message msg = new Message((byte)msg_type.RESPONSE, in_msg.service_name, 0, (byte)status.ALL_PORTS_BUSY);
            int i = 0;
            while (msg.status!=(byte)status.SUCCESS && i<ports.Length)
            {
                if(ports[i].available==true)
                {
                    ports[i].serviceName = in_msg.service_name;
                    ports[i].available = false;
                    msg.port = ports[i].port;
                    msg.status = (byte)status.SUCCESS;
                }
                i++;
            }
            return msg;
        }
        private Message FindService(Message in_msg)
        {
            Message msg = new Message((byte)msg_type.RESPONSE, in_msg.service_name, 0, (byte)status.SERVICE_NOT_FOUND);
            int i = 0;
            while (msg.status != (byte)status.SUCCESS && i < ports.Length)
            {
                if (ports[i].serviceName.SequenceEqual(in_msg.service_name))//ports[i].serviceName == in_msg.service_name)
                {
                    msg.port = ports[i].port;
                    msg.status = (byte)status.SUCCESS;
                }
                i++;
            }
            return msg;
        }
    }
}
