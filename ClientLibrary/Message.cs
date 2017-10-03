using System;
using System.Collections.Generic;
using System.Text;

namespace ClientLibrary
{
        enum msg_type : byte { REQUEST_PORT = 1, LOOKUP_PORT = 2, KEEP_ALIVE = 3, CLOSE_PORT = 4, RESPONSE = 5, PORT_DEAD = 6, STOP = 7 };
        enum status : byte { SUCCESS = 0, SERVICE_IN_US = 1, SERVICE_NOT_FOUND = 2, ALL_PORTS_BUSY = 3, INVALID_ARG = 4, UNDEFINED_ERROR = 5 }
        class Message
        {
            public byte msg_type { get; set; }
            public char[] service_name { get; set; }

            public UInt16 port { get; set; }
            public byte status { get; set; }

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
                byte[] packet = new byte[5 + service_name.Length];
                packet[0] = msg_type;
                packet[1] = (byte)port;
                packet[2] = (byte)(port >> 8);
                packet[3] = status;
                packet[4] = (byte)(service_name.Length);
                byte[] serviceNameBytes = Encoding.UTF8.GetBytes(service_name);
                for (int i = 0; i < serviceNameBytes.Length; i++)
                {
                    packet[5 + i] = serviceNameBytes[i];
                }
                return packet;
            }
            static public Message FromPacket(byte[] packet)
            {
                Message msg = new Message();
                msg.msg_type = packet[0];
                msg.port = BitConverter.ToUInt16(packet, 1);
                msg.status = packet[3];
                int length = packet[4];
                byte[] serviceNameBytes = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    serviceNameBytes[i] = packet[5 + i];
                }
                msg.service_name = ASCIIEncoding.UTF8.GetChars(serviceNameBytes);
                return msg;
            }

            public override string ToString()
            {
                string str = "";
                switch (msg_type)
                {
                    case 1:
                        {
                            str += "msg_type: REQUEST_PORT\n";
                            break;
                        }
                    case 2:
                        {
                            str += "msg_type: LOOKUP_PORT\n";
                            break;
                        }
                    case 3:
                        {
                            str += "msg_type: KEEP_ALIVE\n";
                            break;
                        }
                    case 4:
                        {
                            str += "msg_type: CLOSE_PORT\n";
                            break;
                        }
                    case 5:
                        {
                            str += "msg_type: RESPONSE\n";
                            break;
                        }
                    case 6:
                        {
                            str += "msg_type: PORT_DEAD\n";
                            break;
                        }
                    case 7:
                        {
                            str += "msg_type: STOP\n";
                            break;
                        }
                    default:
                        break;
                }
                str += "service_name: " + new string(service_name) + "\n";
                str += "port: " + port + "\n";

                switch (status)
                {
                    case 0:
                        {
                            str += "Status: SUCCESS\n";
                            break;
                        }
                    case 1:
                        {
                            str += "Status: SERVICE_IN_USE\n";
                            break;
                        }
                    case 2:
                        {
                            str += "Status: SERVICE_NOT_FOUND\n";
                            break;
                        }
                    case 3:
                        {
                            str += "Status: ALL_PORTS_BUSY\n";
                            break;
                        }
                    case 4:
                        {
                            str += "Status: INVALID_ARG\n";
                            break;
                        }
                    case 5:
                        {
                            str += "Status: UNDEFINED_ERROR\n";
                            break;
                        }
                    default:
                        break;
                }
                return str;
            }
        }
}
