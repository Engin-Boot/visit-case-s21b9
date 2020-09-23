using System;
using System.Collections.Generic;


namespace Receiver
{
    public class Hour
    {
        #region Data Members
        int hourNumber;
        List<string> entries;
        int _totalFootfall;

        #region Property 
        public int TotalFootfall
        {
            get 
            { 
                return _totalFootfall; 
            }
        }
        #endregion

        #endregion

        #region Constructor
        public Hour(int h)
        {
            hourNumber = h;
            entries = new List<string>();
            _totalFootfall = 0;
        }
        #endregion

        #region Methods
        public void AddNewEntry(string timeOfEntry)
        {
            entries.Add(timeOfEntry);
            _totalFootfall++;
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
