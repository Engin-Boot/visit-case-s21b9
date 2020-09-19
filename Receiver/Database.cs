﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Receiver
{
    public class Database
    {
        Dictionary<string,Date> days;
        DateTime currentDate;

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

        public Database()
        {
            days = new Dictionary<string, Date>();
        }

        public void addDay(string s)
        {
            Date d = new Date(s);
            days.Add(s,d);
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

        public int[][] getLast7DaysFootfall()
        {
            int[][] lastWeek = new int[7][];
            for(int i=1; i<=7; i++)
            {
                DateTime temp = currentDate.AddDays(-1*(i));
                string d = temp.ToString("dd-MM-yyyy");
                if (days.ContainsKey(d))
                {
                    Date dateToGet = days[d];
                    lastWeek[i-1] = dateToGet.TotalEveryHour;
                }
                else
                {
                    int[] arr = new int[24];
                    lastWeek[i - 1] = arr;
                }
                
            }

            return lastWeek;
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
            String lastDate="";
            foreach (var VARIABLE in newDictionary)
            {
                string [] values = VARIABLE.Value.ToArray();
                for(int i = 0; i < values.Length; i++)
                {
                    addNewEntryToDate(values[i], VARIABLE.Key);
                }
                Console.WriteLine(VARIABLE.Key + "->" + string.Join(",", VARIABLE.Value));
                lastDate = VARIABLE.Key;
            }
            try
            {
                currentDate = DateTime.ParseExact(lastDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return 1;
            }
            return 0;
        }

        public string toString()
        {
            String databaseString = "";

            foreach(var item in days)
            {
                Date t = item.Value;
                databaseString += t.toString();
            }
            return databaseString;
        }

    }
}