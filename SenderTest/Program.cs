using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SenderTest
{
    class Program
    {
        public void WhenFileDoesNotExistThenReturnFalseAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            string correctFilePath = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            //Passing condition
            bool fileExists = sg.CheckIfFileExists(correctFilePath);
            string noErrorMessageFromSender = sg.message;
            string expectedMessage = null;
            Assert.True(fileExists);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Failing condition
            string wrongFilePath = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFile\Visit-record-inputs.csv";
            bool fileDoesNotExists = sg.CheckIfFileExists(wrongFilePath);
            string errorMessageFromSender = sg.message;
            string expectedMessage1 = "The File-> " + wrongFilePath + " Does Not Exists";
            Assert.False(fileDoesNotExists);
            Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        public void WhenFileDoesNotHasValidExtensionThenReturnFalseAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            string correctFileExtension = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            //Passing condition
            bool fileHasValidExtension= sg.CheckIfFileHasValidExtension(correctFileExtension);
            string noErrorMessageFromSender = sg.message;
            string expectedMessage = null;
            Assert.True(fileHasValidExtension);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Failing condition
            string wrongFileExtension = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.xlsx";
            bool fileHasInValidExtension = sg.CheckIfFileHasValidExtension(wrongFileExtension);
            string errorMessageFromSender = sg.message;
            string expectedMessage1 = "The File-> " + wrongFileExtension + " Does Not Have A Valid Extension";
            Assert.False(fileHasInValidExtension);
            Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        public void WhenFileIsEmptyThenReturnTrueAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            string nonEmptyFile = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            //Failing condition
            bool fileIsNotEmpty = sg.CheckIfFileIsEmpty(nonEmptyFile);
            string noErrorMessageFromSender = sg.message;
            string expectedMessage = null;
            Assert.False(fileIsNotEmpty);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Failing condition
            string emptyFile = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\EmptyFile.csv";
            bool fileIsEmpty = sg.CheckIfFileIsEmpty(emptyFile);
            string errorMessageFromSender = sg.message;
            string expectedMessage1 = "The File-> " + emptyFile + " Is Empty";
            Assert.True(fileIsEmpty);
            Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        public void WhenFileIsUnavailableForUseThenReturnTrueAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            string fileNotInUse = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            //Failing condition
            bool fileExists = sg.CheckIfFileIsInUse(fileNotInUse);
            string noErrorMessageFromSender = sg.message;
            string expectedMessage = null;
            Assert.False(fileExists);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Passing condition
            //string fileInUse = @"C:\Users\320087363\Desktop\Bootcamp\Visit-record-inputs.csv";
            //bool fileDoesNotExists = sg.CheckIfFileIsInUse(fileInUse);
            //string errorMessageFromSender = sg.message;
            //string expectedMessage1 = "The File-> " + fileInUse + " Is Being Used By Another Process";
            //Assert.False(fileDoesNotExists);
            //Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        public void WhenAnyRowIsIncompleteThenReturnTrueAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            //Failing condition
            bool dateIsComplete = sg.CheckIfAnyRowHasIncompleteData("1,12-10-2020,12:34:12");
            string noErrorMessageFromSender = sg.message;
            string expectedMessage = null;
            Assert.False(dateIsComplete);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Passing condition
            bool dateIsIncomplete = sg.CheckIfAnyRowHasIncompleteData("1,,12:34:12");
            string errorMessageFromSender = sg.message;
            string expectedMessage1 = "Data is incomplete at row index :- 1";
            Assert.True(dateIsIncomplete);
            Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        static void Main(string[] args)
        {
          Program testSenderData = new Program();
          testSenderData.WhenFileDoesNotExistThenReturnFalseAndSendErrorMessage();
          testSenderData.WhenFileDoesNotHasValidExtensionThenReturnFalseAndSendErrorMessage();
          testSenderData.WhenFileIsEmptyThenReturnTrueAndSendErrorMessage();
          testSenderData.WhenFileIsUnavailableForUseThenReturnTrueAndSendErrorMessage();
          testSenderData.WhenAnyRowIsIncompleteThenReturnTrueAndSendErrorMessage();
        }
    }
}
