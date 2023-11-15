using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace PetStoreApiTest.ExtentReporting
{
    public class ExtentManager
    {
        private static ExtentReports extent;

        public static ExtentReports GetExtent()
        {
            if (extent == null)
            {
                string PathToDirectory = Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory);
                var htmlReporter = new ExtentHtmlReporter(string.Concat(PathToDirectory, "/../../TestReport/Report.html"));
                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);

                htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
                htmlReporter.Config.DocumentTitle = "Petstore";
                htmlReporter.Config.ReportName = "Pet Api Test";
                htmlReporter.Config.EnableTimeline = true;
            }
            return extent;
        }
    }
}