using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenderExtendedFunc
{
    public class Extendedfunc
    {
        private Dictionary<string, List<string>> dataDictionary = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> manualDictionary = new Dictionary<string, List<string>>();
        public bool IsMalfunctioned;

        public Extendedfunc(Dictionary<string, List<string>> dataDictionary,
            Dictionary<string, List<string>> manualDictionary)
        {
            this.dataDictionary = dataDictionary;
            this.manualDictionary = manualDictionary;
        }

        public void SendListOfHoursToCheckForMalfunctionality()
        {
            int count = -1;
            foreach (var VARIABLE in manualDictionary)
            {
                count++;
                if (dataDictionary.ContainsKey(VARIABLE.Key))
                {
                    ListOfHoursToTimeSpan(dataDictionary[VARIABLE.Key], VARIABLE.Value, VARIABLE.Key);
                }
                else
                {
                    this.IsMalfunctioned = true;
                    SendAlarmOnMalfunction("Sensor is malfunctioning for the whole day on " + VARIABLE.Key + " hence reconciling data from manual log");
                    ReconcileDataForWholeDay(VARIABLE.Key, VARIABLE.Value, count);
                }
            }
        }

        public void ListOfHoursToTimeSpan(List<string> v, List<string> m, string key)
        {
            List<TimeSpan> vt = new List<TimeSpan>();
            List<TimeSpan> mt = new List<TimeSpan>();
            string[] formats = { "HH:mm:ss", "H:mm:ss" };

            for (int i = 0; i < v.Count; i++)
            {

                TimeSpan timeSpan = TimeSpan.Parse(v[i]);
                vt.Add(timeSpan);
            }
            for (int i = 0; i < m.Count; i++)
            {

                TimeSpan timeSpan = TimeSpan.Parse(m[i]);
                mt.Add(timeSpan);
            }
            vt.Sort();
            mt.Sort();
            CheckForMalfunctionality(vt, mt, key);
        }

        public void CheckForMalfunctionality(List<TimeSpan> v, List<TimeSpan> m, string key)
        {
            for (int index = 1; index < v.Count; index++)
            {
                TimeSpan diff1 = v[index].Subtract(v[index - 1]);
                if (CheckIfMalfunctioned(m[index], v[index], diff1))
                {
                    this.IsMalfunctioned = true;
                    ReconcileData(v, m, index, key);
                }
            }
        }

        public bool CheckIfMalfunctioned(TimeSpan timeFromManual, TimeSpan timeFromActual, TimeSpan diff)
        {
            TimeSpan time = TimeSpan.Parse("5:00:00");
            if (diff >= time)
            {
                if (timeFromActual == timeFromManual)
                {
                    return false;
                }
                else
                {
                    SendAlarmOnMalfunction("The sensor is malfunctioning at " + timeFromManual +
                                      " hence reconciling the data from manual log");
                    return true;
                }
            }

            return false;
        }

        public void ReconcileData(List<TimeSpan> v, List<TimeSpan> m, int index, string key)
        {
            v.Insert(index, m[index]);
            List<string> newv = new List<string>();
            for (int i = 0; i < v.Count; i++)
            {
                newv.Add(v[i].ToString());
            }

            dataDictionary[key] = newv;
        }

        public void ReconcileDataForWholeDay(string key, List<string> m, int count)
        {
            List<KeyValuePair<string, List<string>>> list = dataDictionary.ToList();
            list.Insert(count, new KeyValuePair<string, List<string>>(key, m));
            dataDictionary = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void WriteFileContentsToConsole()
        {
            string senderData = "";
            senderData = JsonConvert.SerializeObject(dataDictionary, Formatting.Indented);
            Console.WriteLine(senderData);
        }

        public void SendAlarmOnMalfunction(string alarmMessage)
        {
            //WriteAlarmMessageToDictionary(alarmMessage);
        }

        public void WriteAlarmMessageToDictionary(string message)
        {
            if (dataDictionary.ContainsKey("Alarm"))
            {
                dataDictionary["Alarm"].Add(message);
            }
            else
            {
                dataDictionary.Add("Alarm", new List<string>() { message });
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
