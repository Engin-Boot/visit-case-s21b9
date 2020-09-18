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
            while ((input = Console.ReadLine()) != null)
            {
                json += input;
            }
            try
            {
                Dictionary<string, List<string>> newDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
                foreach (var VARIABLE in newDictionary)
                {
                    Console.WriteLine(VARIABLE.Key + "->" + string.Join(",", VARIABLE.Value));
                }
            }
            catch(Exception)
            {
                //Handles the error messages from the sender
                Console.WriteLine("Some error occured from sender's side");
            }

        }
    }
}
