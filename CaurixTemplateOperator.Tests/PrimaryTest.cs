using Microsoft.Office.Interop.Word;
using System.Drawing;
using System.Data.Odbc;
using System.Collections.Generic;
using System;
using CaurixTemplateOperator;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CaurixTemplateOperator.Tests
{
    /// <summary>This class contains parameterized unit tests for Primary</summary>
    [TestClass]
    [PexClass(typeof(Primary))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class PrimaryTest
    {

        /// <summary>Test stub for .ctor(List`1&lt;DbOutput&gt;, OdbcConnection, OdbcCommand, String, OdbcDataAdapter, OdbcDataReader, Boolean, String, Object, ReplaceDictionaryArray, MailWrapper, Form1&amp;)</summary>
        [PexMethod]
        public Primary ConstructorTest(
            List<DbOutput> dbList,
            OdbcConnection odbcConn,
            OdbcCommand command,
            string sql,
            OdbcDataAdapter adapter,
            OdbcDataReader data,
            bool disableEmail,
            string pathSaveTo,
            object wordTemplatePath,
            ReplaceDictionaryArray replaceDictionaryArray,
            Primary.MailWrapper mailWrapper,
            ref Form1 fff
        )
        {
            Primary target = new Primary(dbList, odbcConn, command, sql, adapter, data,
                                         disableEmail, pathSaveTo, wordTemplatePath, replaceDictionaryArray, mailWrapper, ref fff);
            return target;
            // TODO: add assertions to method PrimaryTest.ConstructorTest(List`1<DbOutput>, OdbcConnection, OdbcCommand, String, OdbcDataAdapter, OdbcDataReader, Boolean, String, Object, ReplaceDictionaryArray, MailWrapper, Form1&)
        }

        /// <summary>Test stub for CheckIfToSkip(String)</summary>
        [PexMethod]
        public bool CheckIfToSkipTest([PexAssumeUnderTest]Primary target, string inputIDN)
        {
            bool result = target.CheckIfToSkip(inputIDN);
            return result;
            // TODO: add assertions to method PrimaryTest.CheckIfToSkipTest(Primary, String)
        }

        /// <summary>Test stub for ConnectDb()</summary>
        [PexMethod]
        public void ConnectDbTest([PexAssumeUnderTest]Primary target)
        {
            target.ConnectDb();
            // TODO: add assertions to method PrimaryTest.ConnectDbTest(Primary)
        }

        /// <summary>Test stub for DoMail(String, String)</summary>
        [PexMethod]
        public void DoMailTest(
            [PexAssumeUnderTest]Primary target,
            string filepath,
            string msidn
        )
        {
            target.DoMail(filepath, msidn);
            // TODO: add assertions to method PrimaryTest.DoMailTest(Primary, String, String)
        }

        /// <summary>Test stub for EnumerateAccounts()</summary>
        [PexMethod]
        public void EnumerateAccountsTest([PexAssumeUnderTest]Primary target)
        {
            target.EnumerateAccounts();
            // TODO: add assertions to method PrimaryTest.EnumerateAccountsTest(Primary)
        }

        /// <summary>Test stub for ExportFiles()</summary>
        [PexMethod]
        public void ExportFilesTest([PexAssumeUnderTest]Primary target)
        {
            target.ExportFiles();
            // TODO: add assertions to method PrimaryTest.ExportFilesTest(Primary)
        }

        /// <summary>Test stub for GetFieldsFromDBRecord(OdbcDataReader&amp;)</summary>
        [PexMethod]
        public Dictionary<string, string> GetFieldsFromDBRecordTest([PexAssumeUnderTest]Primary target, ref OdbcDataReader dataReader)
        {
            Dictionary<string, string> result = target.GetFieldsFromDBRecord(ref dataReader);
            return result;
            // TODO: add assertions to method PrimaryTest.GetFieldsFromDBRecordTest(Primary, OdbcDataReader&)
        }

        /// <summary>Test stub for InsertImagesIntoPDF(String, String, Image, Image)</summary>
        [PexMethod]
        public void InsertImagesIntoPDFTest(
            [PexAssumeUnderTest]Primary target,
            string pdfInput,
            string pdfOutput,
            Image signature,
            Image identif
        )
        {
            target.InsertImagesIntoPDF(pdfInput, pdfOutput, signature, identif);
            // TODO: add assertions to method PrimaryTest.InsertImagesIntoPDFTest(Primary, String, String, Image, Image)
        }

        /// <summary>Test stub for InsertImagesIntoWord(Document, String, String)</summary>
        [PexMethod]
        public void InsertImagesIntoWordTest(
            [PexAssumeUnderTest]Primary target,
            Document wDocument,
            string signature,
            string identif
        )
        {
            target.InsertImagesIntoWord(wDocument, signature, identif);
            // TODO: add assertions to method PrimaryTest.InsertImagesIntoWordTest(Primary, Document, String, String)
        }

        /// <summary>Test stub for InsertImagesIntoWord2(Document, ImagePair)</summary>
        [PexMethod]
        public void InsertImagesIntoWord2Test(
            [PexAssumeUnderTest]Primary target,
            Document wDocument,
            ImagePair imagePair
        )
        {
            target.InsertImagesIntoWord2(wDocument, imagePair);
            // TODO: add assertions to method PrimaryTest.InsertImagesIntoWord2Test(Primary, Document, ImagePair)
        }

        /// <summary>Test stub for LoadImageFromEmail(String, String)</summary>
        [PexMethod]
        public string LoadImageFromEmailTest(
            [PexAssumeUnderTest]Primary target,
            string number,
            string nameKey
        )
        {
            string result = target.LoadImageFromEmail(number, nameKey);
            return result;
            // TODO: add assertions to method PrimaryTest.LoadImageFromEmailTest(Primary, String, String)
        }

        /// <summary>Test stub for OrganizerStart()</summary>
        [PexMethod]
        public void OrganizerStartTest([PexAssumeUnderTest]Primary target)
        {
            target.OrganizerStart();
            // TODO: add assertions to method PrimaryTest.OrganizerStartTest(Primary)
        }

        /// <summary>Test stub for PushMessageToForm(String)</summary>
        [PexMethod]
        public void PushMessageToFormTest([PexAssumeUnderTest]Primary target, string m)
        {
            target.PushMessageToForm(m);
            // TODO: add assertions to method PrimaryTest.PushMessageToFormTest(Primary, String)
        }
    }
}
