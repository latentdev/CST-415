using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRS
{
    enum alive : int { DEAD = 0, ALIVE = 1 }
    class Port
    {
        public UInt16 port { get; set; }
        public char[] serviceName { get; set; }
        public int status { get; set; }
        public bool available { get; set; }
        public DateTime timeStamp { get; set; }

        public Port(UInt16 port, int status, bool available = true)
        {
            this.port = port;
            this.status = status;
            this.available = available;
        }
    }
}
