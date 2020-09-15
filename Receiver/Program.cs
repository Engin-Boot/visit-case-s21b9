using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                string input = Console.ReadLine();
                Console.WriteLine(input.ToUpper());
            }
            
        }
    }
}
