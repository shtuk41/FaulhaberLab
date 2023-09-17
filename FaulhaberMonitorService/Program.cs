using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaulhaberMonitorService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Monitor monitor = new Monitor();
                monitor.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine("Main exception handler: " + e.Message);
             }

            Console.WriteLine("\nExiting...");
        }
    }
}
