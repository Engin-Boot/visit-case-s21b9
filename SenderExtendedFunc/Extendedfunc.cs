using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SenderExtendedFunc
{
    public class Extendedfunc
    {
        private Dictionary<string, List<string>> _dataDictionary;
        private Dictionary<string, List<string>> _manualDictionary;
        public bool IsMalfunctioned;

        public Extendedfunc(Dictionary<string, List<string>> dataDictionary,
            Dictionary<string, List<string>> manualDictionary)
        {
            this._dataDictionary = dataDictionary;
            this._manualDictionary = manualDictionary;
        }

        public void SendListOfHoursToCheckForMalfunctionality()
        {
            int count = -1;
            foreach (var variable in _manualDictionary)
            {
                count++;
                if (_dataDictionary.ContainsKey(variable.Key))
                {
                    ListOfHoursToTimeSpan(_dataDictionary[variable.Key], variable.Value, variable.Key);
                }
                else
                {
                    this.IsMalfunctioned = true;
                    SendAlarmOnMalfunction("Sensor is malfunctioning for the whole day on " + variable.Key + " hence reconciling data from manual log");
                    ReconcileDataForWholeDay(variable.Key, variable.Value, count);
                }
            }
        }

        public void ListOfHoursToTimeSpan(List<string> v, List<string> m, string key)
        {
            List<TimeSpan> vt = new List<TimeSpan>();
            List<TimeSpan> mt = new List<TimeSpan>();

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

            _dataDictionary[key] = newv;
        }

        public void ReconcileDataForWholeDay(string key, List<string> m, int count)
        {
            List<KeyValuePair<string, List<string>>> list = _dataDictionary.ToList();
            list.Insert(count, new KeyValuePair<string, List<string>>(key, m));
            _dataDictionary = list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void WriteFileContentsToConsole()
        {
            string senderData = JsonConvert.SerializeObject(_dataDictionary, Formatting.Indented);
            Console.WriteLine(senderData);
        }
        
        //Sends an alarm to the Receiever that the sensor is malfunctioning
        /*public void SendAlarmOnMalfunction(string alarmMessage)
        {
            WriteAlarmMessageToDictionary(alarmMessage);
        }

        public void WriteAlarmMessageToDictionary(string message)
        {
            if (_dataDictionary.ContainsKey("Alarm"))
            {
                _dataDictionary["Alarm"].Add(message);
            }
            else
            {
                _dataDictionary.Add("Alarm", new List<string>() { message });
            }
        }*/

        static void Main()
        {
        }
    }
}
