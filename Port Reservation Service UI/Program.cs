using PRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRS_UI
{
    class Program
    {
        static void Main(string[] args)
        {
            PortReservationService service = new PortReservationService();
            while (true)
            {
                service.Listen();
            }
        }
    }
}
