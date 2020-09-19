using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string json = "";
            Database database = new Database();
            while ((input = Console.ReadLine()) != null)
            {
                json += input;
                //Console.WriteLine(input + i++);
            }
            int populationSuccess = database.populateDatabase(json);

            if(populationSuccess == -1)
            {
                Console.WriteLine("Some error occured from sender's side");
            }

            //Console.WriteLine(database.toString());
            Console.WriteLine("Data populated");

            double[] avgPerHour = database.averageFootfallPerHour();
            
            for(int i = 0; i < avgPerHour.Length; i++)
            {
                Console.WriteLine(i + " Hour" + avgPerHour[i]);
            }

            Console.WriteLine("Average per hour Found");

        }
    }
}
