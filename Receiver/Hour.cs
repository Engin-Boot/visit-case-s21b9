using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    public class Hour
    {
        #region Data Members
        int hourNumber;
        List<string> entries;
        int totalFootfall;

        #region Property 
        public int TotalFootfall
        {
            get { return totalFootfall; }
        }
        #endregion

        #endregion

        #region Constructor
        public Hour(int h)
        {
            hourNumber = h;
            entries = new List<string>();
            totalFootfall = 0;
        }
        #endregion

        #region Methods
        public void addNewEntry(string timeOfEntry)
        {
            entries.Add(timeOfEntry);
            totalFootfall++;
        }

        public string toString()
        {
            String s = hourNumber.ToString() + " : ";
            foreach(var item in entries)
            {
                s += item+" ,";
            }
            return s;
        }
        #endregion

    }
}
