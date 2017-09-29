using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST_415_Assignment_1_Client
{
    enum msg_type : byte { REQUEST_PORT = 1, LOOKUP_PORT, KEEP_ALIVE, CLOSE_PORT, RESPONSE, PORT_DEAD, STOP };
    enum status : byte { SUCCESS, SERVICE_IN_USE, SERVICE_NOT_FOUND, ALL_PORTS_BUSY, INVALID_ARG, UNDEFINED_ERROR }
    class Message
    {
        public byte msg_type { get; set; }
        public char[] service_name { get; set; }

        public UInt16 port { get; set; }
        public byte status{ get; set; }

        private Message()
        {

        }
        public Message(byte msg_type, char[] service_name, UInt16 port, byte status)
        {
            this.msg_type = msg_type;
            this.service_name = service_name;
            this.port = port;
            this.status = status;
        }
        public byte[] ToPacket()
        {
            byte[] packet = new byte[5 + (service_name.Length * 2)];
            packet[0] = msg_type;
            packet[1] = (byte) (port >> 8);
            packet[2] = (byte)port;
            packet[3] = status;
            packet[4] = (byte)(service_name.Length * 2);
            byte[] serviceNameBytes= Encoding.UTF8.GetBytes(service_name);
            for (int i = 0;i<serviceNameBytes.Length;i++)
            {
                packet[5 + i] = serviceNameBytes[i];
            }
            return packet;
        }
        static public Message FromPacket(byte [] packet)
        {
            Message msg = new Message();
            msg.msg_type = packet[0];
            msg.port = BitConverter.ToUInt16(packet, 1);
            msg.status = packet[3];
            int length = packet[4];
            byte[] serviceNameBytes = new byte[length];
            for (int i=0; i<length;i++)
            {
                serviceNameBytes[i] = packet[5 + i];
            }
            msg.service_name = ASCIIEncoding.UTF8.GetChars(serviceNameBytes);
            return msg;
        }
    }
}
