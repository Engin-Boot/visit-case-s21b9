using System;
using Xunit;

namespace SenderTestFunc
{
    public class UnitTest1
    {
        [Fact]
        public void WhenFileDoesNotExistThenReturnFalseAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            string correctFilePath = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            //Passing condition
            bool fileExists = sg.CheckIfFileExists(correctFilePath);
            string noErrorMessageFromSender = sg.Message;
            string expectedMessage = null;
            Assert.True(fileExists);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Failing condition
            string wrongFilePath =
                @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFile\Visit-record-inputs.csv";
            bool fileDoesNotExists = sg.CheckIfFileExists(wrongFilePath);
            string errorMessageFromSender = sg.Message;
            string expectedMessage1 = "The File-> " + wrongFilePath + " Does Not Exists";
            Assert.False(fileDoesNotExists);
            Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        [Fact]
        public void WhenFileDoesNotHasValidExtensionThenReturnFalseAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            string correctFileExtension =
                @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            //Passing condition
            bool fileHasValidExtension = sg.CheckIfFileHasValidExtension(correctFileExtension);
            string noErrorMessageFromSender = sg.Message;
            string expectedMessage = null;
            Assert.True(fileHasValidExtension);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Failing condition
            string wrongFileExtension =
                @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.xlsx";
            bool fileHasInValidExtension = sg.CheckIfFileHasValidExtension(wrongFileExtension);
            string errorMessageFromSender = sg.Message;
            string expectedMessage1 = "The File-> " + wrongFileExtension + " Does Not Have A Valid Extension";
            Assert.False(fileHasInValidExtension);
            Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        [Fact]
        public void WhenFileIsEmptyThenReturnTrueAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            string nonEmptyFile = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            //Failing condition
            bool fileIsNotEmpty = sg.CheckIfFileIsEmpty(nonEmptyFile);
            string noErrorMessageFromSender = sg.Message;
            string expectedMessage = null;
            Assert.False(fileIsNotEmpty);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Failing condition
            string emptyFile = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\EmptyFile.csv";
            bool fileIsEmpty = sg.CheckIfFileIsEmpty(emptyFile);
            string errorMessageFromSender = sg.Message;
            string expectedMessage1 = "The File-> " + emptyFile + " Is Empty";
            Assert.True(fileIsEmpty);
            Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        [Fact]
        public void WhenFileIsUnavailableForUseThenReturnTrueAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            string fileNotInUse = @"D:\a\visit-case-s21b9\visit-case-s21b9\Sender\TestDataFiles\Visit-record-inputs.csv";
            //Failing condition
            bool fileExists = sg.CheckIfFileIsInUse(fileNotInUse);
            string noErrorMessageFromSender = sg.Message;
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

        [Fact]
        public void WhenAnyRowIsIncompleteThenReturnTrueAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            //Failing condition
            bool dateIsComplete = sg.CheckIfAnyRowHasIncompleteData("1,12-10-2020,12:34:12");
            string noErrorMessageFromSender = sg.Message;
            string expectedMessage = null;
            Assert.False(dateIsComplete);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Passing condition
            bool dateIsIncomplete = sg.CheckIfAnyRowHasIncompleteData("1,,12:34:12");
            string errorMessageFromSender = sg.Message;
            string expectedMessage1 = "Data is incomplete at row index :- 1";
            Assert.True(dateIsIncomplete);
            Assert.Equal(expectedMessage1, errorMessageFromSender);
        }

        [Fact]
        public void WhenTheDateTimeIsNotValidThenReturnFalseAndSendErrorMessage()
        {
            Sender.Program sg = new Sender.Program();
            //Passing condition
            bool datetimeIsValid = sg.CheckIfDateTimeIsValidAndHasValidFormat("1,12:34:12,12-10-2020,");
            string noErrorMessageFromSender = sg.Message;
            string expectedMessage = null;
            Assert.True(datetimeIsValid);
            Assert.Equal(expectedMessage, noErrorMessageFromSender);

            //Failing condition1
            bool datetimeIsNotValid1 = sg.CheckIfDateTimeIsValidAndHasValidFormat("1,12:34:12,12-20-2020");
            string Invaliddatetime1 = "12-20-2020 12:34:12";
            string errorMessageFromSender1 = sg.Message;
            string expectedMessage11 = "Invalid DateTime Format -> " + Invaliddatetime1 + " at row index -> 1";
            Assert.False(datetimeIsNotValid1);
            Assert.Equal(expectedMessage11, errorMessageFromSender1);

            //Failing condition2
            bool datetimeIsNotValid2 = sg.CheckIfDateTimeIsValidAndHasValidFormat("1,12:67:12,12-10-2020");
            string Invaliddatetime2 = "12-10-2020 12:67:12";
            string errorMessageFromSender2 = sg.Message;
            string expectedMessage12 = "Invalid DateTime Format -> " + Invaliddatetime2 + " at row index -> 1";
            Assert.False(datetimeIsNotValid2);
            Assert.Equal(expectedMessage12, errorMessageFromSender2);
        }
    }
}
