using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Receiver
{
    public class Date
    {
        #region Data Members
        DateTime _date;
        Hour[] hourList;
        int[] totalEveryHour;
        //int totalFootfall;

        #endregion

        #region Properties
        public int[] TotalEveryHour
        {
            get 
            { 
                return totalEveryHour; 
            }
        }
        #endregion

        #region Constructor and initializations

        public Date (string d)
        {
            try
            {
                var formatStrings = new [] { "dd-MM-yyyy", "d-MM-yyyy" };
                DateTime myDate = DateTime.ParseExact(d, formatStrings, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None); 
                _date = myDate;
                totalEveryHour = new int[24];
                hourList = new Hour[24];
                InitializeHourList();
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error in converting data: "+ e.ToString());

            }

        }

        void InitializeHourList()
        {
            for(int i = 0; i < 24; i++)
            {
                Hour temp = new Hour(i);
                hourList[i] = temp;
                //Console.WriteLine(temp.toString());
            }
            
        }
        #endregion

        #region Methods
        public void AddNewEntry(String newEntry)
        {
            
            try
            {
                var formatStrings = new [] { "HH:mm:ss", "h:mm:ss" };
                DateTime newEntryTime = DateTime.ParseExact(newEntry, formatStrings, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None);
                int h = newEntryTime.Hour;
                hourList[h].AddNewEntry(newEntry);
                totalEveryHour[h] = hourList[h].TotalFootfall;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(newEntry + "   .."+_date.ToString()+"..   " + e.ToString());
            }
            
        }

        public string toString()
        {
            String s = _date.ToString("dd-MM-yyyy") + " -->  "+ '\n';
            for (int i = 0; i < hourList.Length; i++)
                foreach(var hr in hourList)
            {
                s = s + hr.toString()+'\n';
            }
            s += '\n';
            return s;
        }

        #endregion
    }

    
}
