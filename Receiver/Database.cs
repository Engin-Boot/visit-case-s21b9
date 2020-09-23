using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Receiver
{
    public class Database
    {
        #region Data Members
        Dictionary<string, Date> days;
        DateTime _currentDate;
        #endregion

        /*
        #region Properties
        public DateTime CurrentDate
        {
            get
            {
                return _currentDate;
            }
        }
        #endregion
        */

        #region Constructor
        public Database()
        {
            days = new Dictionary<string, Date>();
        }
        #endregion

        #region Methods 
        private void AddDay(string s)
        {
            Date d = new Date(s);
            days.Add(s, d);
        }

        private void AddNewEntryToDate(string newEntry, string date)
        {
            if (days.ContainsKey(date))
            {
                days[date].AddNewEntry(newEntry);
            }
            else
            {
                AddDay(date);
                days[date].AddNewEntry(newEntry);
            }
        }

        private int[][] GetLastNWeeksFootfall(int n)
        {
            int numberOfDays = n * 7;
            if (numberOfDays > 0)
            {
                int[][] lastWeek = GetLastNDaysFootfall(numberOfDays);
                return lastWeek;
            }
            int[][] err = new int[][]{ new int[] { 1 } };
            return err;
        }


        private int[][] GetLastNDaysFootfall(int n)
        {
            int[][] nDaysData = new int[n][];
            for (int i = 0; i < n; i++)
            {
                DateTime temp = _currentDate.AddDays(-1 * (i));
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

        public int PopulateDatabase(string data)
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
            foreach (var variable in newDictionary)
            {
                string[] values = variable.Value.ToArray();
                foreach(var val in values)
                {
                    AddNewEntryToDate(val, variable.Key);
                }
                //Console.WriteLine(VARIABLE.Key + "->" + string.Join(",", VARIABLE.Value));
                lastDate = variable.Key;
            }
            SetCurrentDateAs(lastDate);
            return 0;
        }

        private void SetCurrentDateAs(string date)
        {
            try
            {
                _currentDate = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
        

        private double[] GetAverageOfAllArrays(int[][] arrays)
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

        public double[] AverageFootfallPerHour()
        {
            int[][] last7days = GetLastNWeeksFootfall(1);
            double[] average = GetAverageOfAllArrays(last7days);
            return average;
        }

        private double[] GetDailyTotal(int[][] arr)
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


        public double[] AveragePerDayForWeek()
        {
            int NumberOfWeeks = 4;
            int[][] lastWeeks = GetLastNWeeksFootfall(NumberOfWeeks);
            double[] dailyTotal = GetDailyTotal(lastWeeks);
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

        public int[] PeakDailyFootfall()
        {
            int numDays = 30;
            int[][] lastMonth = GetLastNDaysFootfall(numDays);
            double[] dailyTotal = GetDailyTotal(lastMonth);
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
