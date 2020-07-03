using System;
using CaurixTemplateOperator;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CaurixTemplateOperator.Tests
{
    /// <summary>This class contains parameterized unit tests for Form1</summary>
    [TestClass]
    [PexClass(typeof(Form1))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class Form1Test
    {

        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        public Form1 ConstructorTest()
        {
            Form1 target = new Form1();
            return target;
            // TODO: add assertions to method Form1Test.ConstructorTest()
        }

        /// <summary>Test stub for FolderBrowserAsync()</summary>
        [PexMethod]
        public void FolderBrowserAsyncTest([PexAssumeUnderTest]Form1 target)
        {
            target.FolderBrowserAsync();
            // TODO: add assertions to method Form1Test.FolderBrowserAsyncTest(Form1)
        }

        /// <summary>Test stub for InvokerRunner(DateTime)</summary>
        [PexMethod]
        public void InvokerRunnerTest([PexAssumeUnderTest]Form1 target, DateTime nextTime)
        {
            target.InvokerRunner(nextTime);
            // TODO: add assertions to method Form1Test.InvokerRunnerTest(Form1, DateTime)
        }

        /// <summary>Test stub for PushToLabel(String)</summary>
        [PexMethod]
        public void PushToLabelTest([PexAssumeUnderTest]Form1 target, string m)
        {
            target.PushToLabel(m);
            // TODO: add assertions to method Form1Test.PushToLabelTest(Form1, String)
        }

        /// <summary>Test stub for PushToStatus(String)</summary>
        [PexMethod]
        public void PushToStatusTest([PexAssumeUnderTest]Form1 target, string m)
        {
            target.PushToStatus(m);
            // TODO: add assertions to method Form1Test.PushToStatusTest(Form1, String)
        }

        /// <summary>Test stub for StartScheduler()</summary>
        [PexMethod]
        public void StartSchedulerTest([PexAssumeUnderTest]Form1 target)
        {
            target.StartScheduler();
            // TODO: add assertions to method Form1Test.StartSchedulerTest(Form1)
        }

        /// <summary>Test stub for StopScheduler()</summary>
        [PexMethod]
        public void StopSchedulerTest([PexAssumeUnderTest]Form1 target)
        {
            target.StopScheduler();
            // TODO: add assertions to method Form1Test.StopSchedulerTest(Form1)
        }
    }
}
