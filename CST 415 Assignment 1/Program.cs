using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST_415_Assignment_1
{
    class Program
    {
        static void Main(string[] args)
        {
            ReservationService service = new ReservationService();
            while (true)
            {
                service.Listen();
            }
        }
    }
}
