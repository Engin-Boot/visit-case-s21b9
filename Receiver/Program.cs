using System;

namespace Receiver
{
    class Program
    {
        static void Functionalities(Database database)
        {
            double[] avgPerHour = database.AverageFootfallPerHour();

            for (int i = 0; i < avgPerHour.Length; i++)
            {
                Console.WriteLine(i + " Hour" + avgPerHour[i]);
            }

            Console.WriteLine("Average per hour Found");

            double[] avgDailyFootfall = database.AveragePerDayForWeek();
            for (int i = 0; i < avgDailyFootfall.Length; i++)
            {
                Console.WriteLine(i + " Day " + avgDailyFootfall[i]);
            }

            Console.WriteLine("Average daily footfall per week Found");

            int[] peakDaily = database.PeakDailyFootfall();
            Console.WriteLine("Peak daily footfall was " + peakDaily[0] + " '" + peakDaily[1] + "' days before");

        }
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
            int populationSuccess = database.PopulateDatabase(json);

            if(populationSuccess == -1)
            {
                Console.WriteLine("Some error occured from sender's side");
            }

            //Console.WriteLine(database.toString());
            Console.WriteLine("Data populated");

            Functionalities(database);

        }
    }
}
