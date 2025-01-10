using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Tests
{
    public class LeavePageTests
    {
        private IWebDriver _driver;
        private GlobalHelpers _globalHelpers;
        private GlobalLocators _globalLocators;
        private LeavePage _leavePage;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, new Random(), _globalLocators);
            _leavePage = new LeavePage(_driver, _globalHelpers, _globalLocators);
            _globalHelpers.SetLanguageIfNotEnglish();
        }

        //[TestCase(1)]
        [TestCase(5)]
        //[TestCase(14)]
        public void CanApplyForLeave(int duration)
        {
            (var startDate, var endDate) = _leavePage.GetRandomLeaveDates(duration);
            var mustMatchEmployeeName = true;
            _leavePage.ApplyForLeave(startDate, endDate);

            // Check that record exists in main Leave List and My Leave list.
            _leavePage.GotoLeaveList();
            var leaveRecords = _leavePage.FindRecordsForDateRangeAndStatus(startDate, endDate, "Pending", mustMatchEmployeeName);
            Assert.That(leaveRecords.Count, Is.EqualTo(1));

            _leavePage.MyLeaveLink.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.RecordsTable.Displayed);
            leaveRecords = _leavePage.FindRecordsForDateRangeAndStatus(startDate, endDate, "Pending", mustMatchEmployeeName);
            Assert.That(leaveRecords.Count, Is.EqualTo(1));

            // Cancel leave and confirm it now shows as cancelled
            var numCancelledLeaveRecordsForDateRange =
                _leavePage.FindRecordsForDateRangeAndStatus(startDate, endDate, "Cancelled").Count;
            _leavePage.CancelLeave();
            leaveRecords = _leavePage.FindRecordsForDateRangeAndStatus(startDate, endDate, "Cancelled");
            Assert.That(leaveRecords.Count, Is.GreaterThan(numCancelledLeaveRecordsForDateRange));
            _leavePage.GotoLeaveList();
            leaveRecords = _leavePage.FindRecordsForDateRangeAndStatus(startDate, endDate, "Pending", mustMatchEmployeeName);
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
