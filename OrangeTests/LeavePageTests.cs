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
            (bool recordExists, string leaveStatus, IWebElement leaveRecord) = (false, null, null);

            _leavePageHelpers.ApplyForLeave(startDate, endDate);

            // Check that record exists in main Leave List and My Leave list.
            _leavePage.LeaveListLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.RecordCountSpan.Displayed);
            (recordExists, leaveStatus, leaveRecord) = _leavePageHelpers.GetLeaveRecordForDateRange(startDate, endDate);
            _leavePageHelpers.AssertRecordExistsWithStatus(recordExists, leaveRecord, leaveStatus, "Pending");
            
            _leavePage.MyLeaveLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.RecordCountSpan.Displayed);
            _leavePageHelpers.FindRecordByDateRangeAndStatus(startDate, endDate, "Pending");
            Task.Delay(5000);
            (recordExists, leaveStatus, leaveRecord) = _leavePageHelpers.GetLeaveRecordForDateRange(startDate, endDate);
            _leavePageHelpers.AssertRecordExistsWithStatus(recordExists, leaveRecord, leaveStatus, "Pending");

            // Cancel leave and confirm it now shows as cancelled
            _leavePage.CancelLeaveButton.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
            _globalHelpers.Wait.Until(d => _leavePage.LeaveRecords.Count > 0);
            (recordExists, leaveStatus, leaveRecord) = _leavePageHelpers.GetLeaveRecordForDateRange(startDate, endDate);
            _leavePageHelpers.AssertRecordExistsWithStatus(recordExists, leaveRecord, leaveStatus, "Cancelled");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
