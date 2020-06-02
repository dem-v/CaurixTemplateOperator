using System.Drawing;
// <copyright file="ProgramTest.cs">Copyright ©  2020</copyright>

using System;
using System.IO;
using CaurixTemplateOperator;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CaurixTemplateOperator.Tests
{
    [TestClass]
    [PexClass(typeof(Program))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ProgramTest
    {
        /*[PexMethod]
        [PexAllowedException(typeof(TypeInitializationException))]
        public object LoadImageFromEmail(string number, string nameKey)
        {
            //object result = Program.LoadImageFromEmail(number, nameKey);
            //return result;
            return null;
            // TODO: add assertions to method ProgramTest.LoadImageFromEmail(String, String)
        }*/

        /// <summary>Test stub for InsertImagesIntoPDF(String, String, Image, Image)</summary>
        ///
        /// 
        [PexMethod]
        internal void tester()
        {
            InsertImagesIntoPDFTest(@"C:\Users\Demi\Downloads\OutputCaurixFormulaire\770571889.pdf", @"C:\Users\Demi\Downloads\OutputCaurixFormulaire\770571889.pdfTestnew.pdf", Image.FromFile(@"C:\Users\Demi\Downloads\OutputCaurixFormulaire\770571889identif"), Image.FromFile(@"C:\Users\Demi\Downloads\OutputCaurixFormulaire\770571889signature"));
        }

        
        internal void InsertImagesIntoPDFTest(
            string pdfInput,
            string pdfOutput,
            Image signature,
            Image identif
        )
        {
            var a = File.GetLastWriteTime(pdfOutput);
            Program.InsertImagesIntoPDF(pdfInput, pdfOutput, signature, identif);
            // TODO: add assertions to method ProgramTest.InsertImagesIntoPDFTest(String, String, Image, Image)
            var b = File.GetLastWriteTime(pdfOutput);
            PexAssert.AreNotEqual(a, b);
        }
    }
}
