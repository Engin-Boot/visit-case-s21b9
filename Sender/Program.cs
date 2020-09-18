﻿using Newtonsoft.Json;
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
        private Dictionary<string, List<string>> dataDictionary = new Dictionary<string, List<string>>();
        public string message;

        //Check if the file exists or the path to the file is correct
        public bool CheckIfFileExists(string filepath)
        {
            if (File.Exists(filepath))
            {
                return true;
            }
            WriteErrorMessageToDictionary("The File-> " + filepath + " Does Not Exists");
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
                WriteErrorMessageToDictionary("The File-> " + filepath + " Does Not Have A Valid Extension");
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
                    WriteErrorMessageToDictionary("The File-> " + filepath + " Is Empty");
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
                    WriteErrorMessageToDictionary("The File-> " + filepath + " Is Being Used By Another Process");
                    return true;
                }
            }
            return true;
        }

        //Check if the datetime is valid and has valid format
        public bool CheckIfDateTimeIsValidAndHasValidFormat(string value)
        {
            if (!CheckIfAnyRowHasIncompleteData(value))
            {
                string[] columns = value.Split(',');
                string datetime = columns[2] + " " + columns[1];
                DateTime parsedTime;
                string[] formats = { "dd-MM-yyyy HH:mm:ss", "d-MM-yyyy H:mm:ss" };
                var isValidFormat = DateTime.TryParseExact(datetime, formats, new CultureInfo("en-GB"),
                    DateTimeStyles.None, out parsedTime);
                if (!isValidFormat)
                {
                    WriteErrorMessageToDictionary("Invalid DateTime Format -> " + datetime + " at row index -> " +
                                                  columns[0]);
                    return false;
                }

                return true;
            }
            return false;
        }

        //Check if any row has incomplete data
        public bool CheckIfAnyRowHasIncompleteData(string output)
        {
            string[] columns = output.Split(',');
            if (columns[1] == "" | columns[2] == "")
            {
                WriteErrorMessageToDictionary("Data is incomplete at row index :- " + columns[0]);
                return true;
            }
            return false;
        }

        //Read the csv file and send to the receiver
        public void FileReader(string filepath)
        {
            //Perform the read operation

            using (StreamReader streamReader = new StreamReader(filepath))
            {
                streamReader.ReadLine();
                while (!streamReader.EndOfStream)
                {
                    string output = streamReader.ReadLine();
                    string[] columns = output.Split(',');
                    string date = columns[2];
                    string time = columns[1];
                    if (CheckIfDateTimeIsValidAndHasValidFormat(output))
                    {
                        WriteDataToDictionary(date, time);
                    }
                }
            }
        }

        //Add the contents of the csv file to the dictionary
        public void WriteDataToDictionary(string date, string time)
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

        //Writes a message to the console for the error while reading the file
        public void WriteErrorMessageToDictionary(string message)
        {
            this.message = message;
            if (dataDictionary.ContainsKey("Error"))
            {
                dataDictionary["Error"].Add(message);
            }
            else
            {
                dataDictionary.Add("Error", new List<string>() { message });
            }
        }

        //Serialize dictionary object to json string and write it on the console
        public void WriteFileContentsToConsole(string filepath)
        {
            string senderData;
            if (!CheckIfFileIsInUse(filepath))
            {
                //In case of sending data or errors or both
                FileReader(filepath);
                senderData = JsonConvert.SerializeObject(dataDictionary, Formatting.Indented);
            }
            else
            {
                //In case of sending only errors
                senderData = JsonConvert.SerializeObject(dataDictionary, Formatting.Indented);
            }
            Console.WriteLine(senderData);
        }

        static void Main()
        {
            Program senderObj = new Program();
            string filepath = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            senderObj.WriteFileContentsToConsole(filepath);
        }
    }
}
