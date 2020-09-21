using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Receiver
{
    public class Database
    {
        #region Data Members
        Dictionary<string, Date> days;
        DateTime currentDate;
        #endregion

        #region Properties
        public DateTime CurrentDate
        {
            get
            {
                return currentDate;
            }
            set
            {
                currentDate = value;
            }
        }
        #endregion


        #region Constructor
        public Database()
        {
            days = new Dictionary<string, Date>();
        }
        #endregion

        #region Methods 
        public void addDay(string s)
        {
            Date d = new Date(s);
            days.Add(s, d);
        }

        public void addNewEntryToDate(string newEntry, string date)
        {
            if (days.ContainsKey(date))
            {
                days[date].addNewEntry(newEntry);
            }
            else
            {
                addDay(date);
                days[date].addNewEntry(newEntry);
            }
        }

        public int[][] getLastNWeeksFootfall(int N)
        {
            int days = N * 7;
            if (days > 0)
            {
                int[][] lastWeek = getLastNDaysFootfall(days);
                return lastWeek;
            }
            int[][] err = new int[1][];
            err[0] = new int[] { 1 };
            return err;
        }


        public int[][] getLastNDaysFootfall(int N)
        {
            int[][] nDaysData = new int[N][];
            for (int i = 0; i < N; i++)
            {
                DateTime temp = currentDate.AddDays(-1 * (i));
                string d = temp.ToString("dd-MM-yyyy");
                if (days.ContainsKey(d))
                {
                    Date dateToGet = days[d];
                    nDaysData[i] = dateToGet.TotalEveryHour;
                }
                else
                {
                    int[] arr = new int[24];
                    nDaysData[i] = arr;
                }

            }

            return nDaysData;
        }

        public int populateDatabase(string data)
        {
            Dictionary<string, List<string>> newDictionary;
            try
            {
                newDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(data);
            }
            catch
            {
                return -1;
            }
            String lastDate = "";
            foreach (var VARIABLE in newDictionary)
            {
                string[] values = VARIABLE.Value.ToArray();
                for (int i = 0; i < values.Length; i++)
                {
                    addNewEntryToDate(values[i], VARIABLE.Key);
                }
                //Console.WriteLine(VARIABLE.Key + "->" + string.Join(",", VARIABLE.Value));
                lastDate = VARIABLE.Key;
            }
            setCurrentDateAs(lastDate);
            return 0;
        }

        public void setCurrentDateAs(string date)
        {
            try
            {
                currentDate = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                Console.WriteLine("Error in parsing string to date");
            }
        }

        public string toString()
        {
            String databaseString = "";

            foreach (var item in days)
            {
                Date t = item.Value;
                databaseString += t.toString();
            }
            return databaseString;
        }

        public double[] getAverageOfAllArrays(int[][] arrays)
        {
            int len = arrays.Length;
            double[] result = new double[24];
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < arrays[i].Length; j++)
                {
                    result[j] += (arrays[i][j] / (double)len);
                }
            }

            return result;
        }

        public double[] averageFootfallPerHour()
        {
            int[][] last7days = getLastNWeeksFootfall(1);
            double[] average = getAverageOfAllArrays(last7days);
            return average;
        }

        public double[] getDailyTotal(int[][] arr)
        {
            double[] total = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null)
                {
                    total[i] = 0;
                }
                else
                {
                    total[i] = arr[i].Sum();
                }
            }
            return total;
        }


        public double[] averagePerDayForWeek()
        {
            int NumberOfWeeks = 4;
            int[][] lastWeeks = getLastNWeeksFootfall(NumberOfWeeks);
            double[] dailyTotal = getDailyTotal(lastWeeks);
            int daysInWeek = 7;
            double[] average = new double[daysInWeek];

            for (int i = 0; i < 7; i++)
            {
                double sum = 0;
                for (int j = 0; j < NumberOfWeeks; j++)
                {
                    sum += dailyTotal[i + (j * daysInWeek)];
                }
                sum = sum / NumberOfWeeks;
                average[i] = sum;
            }

            return average;

        }

        public int[] peakDailyFootfall()
        {
            int numDays = 30;
            int[][] lastMonth = getLastNDaysFootfall(numDays);
            double[] dailyTotal = getDailyTotal(lastMonth);
            double max = dailyTotal[0];
            int numberOfDaysBefore = 0;
            for (int i = 1; i < dailyTotal.Length; i++)
            {
                if (dailyTotal[i] > max)
                {
                    max = dailyTotal[i];
                    numberOfDaysBefore = i;
                }
            }
            int[] result = new int[2];
            result[0] = (int)max;
            result[1] = numberOfDaysBefore;
            return result;
        }
        #endregion

    }
}
