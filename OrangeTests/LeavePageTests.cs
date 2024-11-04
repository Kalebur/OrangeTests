using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests
{
    public class LeavePageTests
    {
        private IWebDriver _driver;
        private GlobalHelpers _globalHelpers;
        private GlobalLocators _globalLocators;
        private LeavePage _leavePage;
        private LeavePageHelpers _leavePageHelpers;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, new Random(), _globalLocators);
            _leavePage = new LeavePage(_driver);
            _leavePageHelpers = new LeavePageHelpers(_leavePage, _globalHelpers, _globalLocators);
        }

        //[TestCase(1)]
        [TestCase(5)]
        //[TestCase(14)]
        public void CanApplyForLeave(int duration)
        {
            (var startDate, var endDate) = _leavePageHelpers.GetRandomLeaveDates(duration);
            var mustMatchEmployeeName = true;
            _leavePageHelpers.ApplyForLeave(startDate, endDate);

            // Check that record exists in main Leave List and My Leave list.
            _leavePageHelpers.GotoLeaveList();
            var leaveRecords = _leavePageHelpers.FindRecordsForDateRangeAndStatus(startDate, endDate, "Pending", mustMatchEmployeeName);
            Assert.That(leaveRecords.Count, Is.EqualTo(1));
            
            _leavePage.MyLeaveLink.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.RecordsTable.Displayed);
            leaveRecords = _leavePageHelpers.FindRecordsForDateRangeAndStatus(startDate, endDate, "Pending", mustMatchEmployeeName);
            Assert.That(leaveRecords.Count, Is.EqualTo(1));

            // Cancel leave and confirm it now shows as cancelled
            var numCancelledLeaveRecordsForDateRange = 
                _leavePageHelpers.FindRecordsForDateRangeAndStatus(startDate, endDate, "Cancelled").Count;
            _leavePageHelpers.CancelLeave();
            leaveRecords = _leavePageHelpers.FindRecordsForDateRangeAndStatus(startDate, endDate, "Cancelled");
            Assert.That(leaveRecords.Count, Is.GreaterThan(numCancelledLeaveRecordsForDateRange));
            _leavePageHelpers.GotoLeaveList();
            leaveRecords = _leavePageHelpers.FindRecordsForDateRangeAndStatus(startDate, endDate, "Pending", mustMatchEmployeeName);
            Assert.That(leaveRecords.Count, Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
