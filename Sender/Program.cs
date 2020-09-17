using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        //Sends a message to the console for the error while reading the file
        public void SendErrorMessage(string message)
        {
            Console.WriteLine(message);
        }

        //Check if the file exists or the path to the file is correct
        public bool CheckIfFileExists(string filepath)
        {
            if (File.Exists(filepath))
            {
                return true;
            }
            SendErrorMessage("The File-> " + filepath + " Does Not Exists");
            return false;
        }

        //Check if the file extension is valid
        public bool CheckIfFileHasValidExtension(string filepath)
        {
            if (CheckIfFileExists(filepath))
            {
                if (filepath.EndsWith(".csv"))
                {
                    return true;
                }
                SendErrorMessage("The File-> " + filepath + " Does Not Have A Valid Extension");
            }
            return false;
        }

        //Check if the file is empty
        public bool CheckIfFileIsEmpty(string filepath)
        {
            if (CheckIfFileHasValidExtension(filepath))
            {
                if (new FileInfo(filepath).Length == 0)
                {
                    SendErrorMessage("The File-> " + filepath + " Is Empty");
                    return true;
                }

                return false;
            }
            return true;
        }

        //Check if the file is available for use
        public bool CheckIfFileIsInUse(string filepath)
        {
            if (!CheckIfFileIsEmpty(filepath))
            {
                try
                {
                    FileStream fs = File.Open(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    fs.Close();
                    return false;
                }
                catch (IOException)
                {
                    SendErrorMessage("The File-> " + filepath + " Is Being Used By Another Process");
                    return true;
                }
            }
            return true;
        }

        //Check if the date is valid and has a valid format
        public bool CheckIfDateIsValidAndHasValidFormat(string value)
        {
            string[] formats = { "d/MM/yyyy", "dd/MM/yyyy", "dd-MM-yyyy", "dd.MM.yyyy" };
            DateTime parsedDate;
            var isValidFormat = DateTime.TryParseExact(value, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDate);
            if (isValidFormat)
            {
                return true;
            }
            else
            {
                SendErrorMessage("Invalid Date -> " + value);
                return false;
            }
        }

        //Check if the time is valid and has valid format
        public bool CheckIfTimeIsValidAndHasValidFormat(string value)
        {
            string[] formats = { "hh:mm:ss" };
            DateTime parsedTime;
            var isValidFormat = DateTime.TryParseExact(value, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedTime);
            if (isValidFormat)
            {
                return true;
            }
            else
            {
                SendErrorMessage("Invalid Time -> " + value);
                return false;
            }

        }

        //Check if any row has incomplete data
        public bool CheckIfAnyRowHasIncompleteData(string value1, string value2)
        {
            if (value1 == "" | value2 == "")
            {
                return true;
            }

            return false;
        }

        //Read the csv file and send to the receiver
        public string FileReader(string filepath)
        {
            //Perform the read operation
            Dictionary<string, List<string>> dataDictionary = new Dictionary<string, List<string>>();
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                while (!streamReader.EndOfStream)
                {
                    string output = streamReader.ReadLine();
                    string[] columns = output.Split(',');
                    string date = columns[2];
                    string time = columns[1];
                    string rowindex = columns[0];
                    if (!CheckIfAnyRowHasIncompleteData(date, time))
                    {
                        if (dataDictionary.ContainsKey(date))
                        {
                            dataDictionary[date].Add(time);
                        }
                        else
                        {
                            dataDictionary.Add(date, new List<string>());
                            dataDictionary[date].Add(time);
                        }
                    }
                    else
                    {
                        SendErrorMessage("Data is incomplete at row index -> " + rowindex);
                    }
                }
            }
            string jsondata = JsonConvert.SerializeObject(dataDictionary, Formatting.Indented);
            return jsondata;
        }

        //Writes the contents of the csv file to the console
        public void WriteFileContentsToConsole(string filepath)
        {

            if (!CheckIfFileIsInUse(filepath))
            {
                string senderData = FileReader(filepath);
                Console.WriteLine(senderData);
            }
        }

        static void Main()
        {
            Program pobj = new Program();
            string filepath = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            pobj.WriteFileContentsToConsole(filepath);
            //Console.ReadKey();
        }
    }
}
