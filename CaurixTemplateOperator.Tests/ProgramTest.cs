// <copyright file="ProgramTest.cs">Copyright ©  2020</copyright>

using System;
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

        [PexMethod]
        [PexAllowedException(typeof(TypeInitializationException))]
        public object LoadImageFromEmail(string number, string nameKey)
        {
            //object result = Program.LoadImageFromEmail(number, nameKey);
            //return result;
            return null;
            // TODO: add assertions to method ProgramTest.LoadImageFromEmail(String, String)
        }
    }
}
