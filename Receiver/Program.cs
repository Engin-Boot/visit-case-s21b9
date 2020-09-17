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
            Dictionary<string, List<string>> newDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            foreach (var VARIABLE in newDictionary)
            {
                Console.WriteLine(VARIABLE.Key + "->" + string.Join(",", VARIABLE.Value));
            }

        }
    }
}
