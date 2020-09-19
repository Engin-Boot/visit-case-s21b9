using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Receiver
{
    public class Date
    {
        #region Data Members
        DateTime date;
        Hour[] hourList;
        int[] totalEveryHour;

        #endregion

        #region Properties
        public int[] TotalEveryHour
        {
            get { return totalEveryHour; }
        }
        #endregion
        #region Constructor and initializations

        public Date (string d)
        {
            try
            {
                var formatStrings = new string[] { "dd-MM-yyyy", "d-MM-yyyy" };
                DateTime myDate = DateTime.ParseExact(d, formatStrings, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None); 
                date = myDate;
                totalEveryHour = new int[24];
                hourList = new Hour[24];
                InitializeHourList();
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error in converting data: ", e.ToString());

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
        public void addNewEntry(String newEntry)
        {
            DateTime newEntryTime;
            try
            {
                var formatStrings = new string[] { "HH:mm:ss", "h:mm:ss" };
                newEntryTime = DateTime.ParseExact(newEntry, formatStrings, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None);
                int h = newEntryTime.Hour;
                hourList[h].addNewEntry(newEntry);
                totalEveryHour[h] = hourList[h].TotalFootfall;
            }
            catch (Exception e)
            {
                Console.WriteLine(newEntry + "   .."+date.ToString()+"..   " + e.ToString());
            }
            
        }

        public string toString()
        {
            String s = date.ToString("dd-MM-yyyy") + " -->  "+ '\n';
            for (int i = 0; i < hourList.Length; i++)
            {
                Hour t = hourList[i];
                s = s + t.toString()+'\n';
            }
            s += '\n';
            return s;
        }

        #endregion
    }

    
}
