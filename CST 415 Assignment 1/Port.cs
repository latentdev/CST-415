using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST_415_Assignment_1
{
    enum alive: int { DEAD=0, ALIVE=1 }
    class Port
    {
        int port { get; set; }
        char[] serviceName { get; set; }
        int status { get; set; }
        public Port(int port, int status)
        {
            this.port = port;
            this.status = status;
        }
    }
}
