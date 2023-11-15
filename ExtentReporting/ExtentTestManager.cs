using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using NUnit.Framework;

namespace PetStoreApiTest.ExtentReporting
{
    public class ExtentTestManager
    {
        public static ExtentReports extent = ExtentManager.GetExtent();
        public static ExtentTest Test;

        public static ExtentTest CreateTest(string testName)
        {
            Test = extent.CreateTest(testName);
            return Test;
        }

        public static ExtentTest TestLogs(string Step)
        {
            return Test.Log(Status.Info, Step);
        }

        public static ExtentTest StatusOfTest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            switch (status)
            {
                case TestStatus.Failed:
                    return Test.Log(Status.Fail, "Test failed: " + errorMessage);
                case TestStatus.Passed:
                    return Test.Log(Status.Pass, "Test passed.");
                case TestStatus.Skipped:
                    return Test.Log(Status.Skip, "Test skipped.");
                default:
                    return Test.Log(Status.Warning, "Test ended with an unexpected status.");
            }
        }
    }
}