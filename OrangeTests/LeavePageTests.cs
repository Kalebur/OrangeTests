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
        private LoginHelpers _loginHelpers;
        private LeavePage _leavePage;
        private LeavePageHelpers _leavePageHelpers;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, new Random(), _globalLocators);
            _loginHelpers = new LoginHelpers(_driver, new LoginPage(_driver), _globalHelpers, _globalLocators);
            _leavePage = new LeavePage(_driver);
            _leavePageHelpers = new LeavePageHelpers(_leavePage, _globalHelpers);
        }

        [Test]
        public void CanApplyForLeave()
        {
            var startDate = new DateTime(2024, 11, 5);
            var endDate = new DateTime(2024, 11, 6);
            (bool recordExists, string leaveStatus, IWebElement leaveRecord) = (false, null, null);

            _loginHelpers.LoginAs("admin", true);
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);
            _globalLocators.LeaveLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyLink.Displayed);
            _leavePage.ApplyLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyButton.Displayed);
            _leavePage.LeaveTypeSelectElement.Click();
            _leavePageHelpers.SelectRandomLeaveType();

            // Select Start Date
            _leavePage.FromDateInputField.SendKeys(startDate.ToString("yyyy-dd-MM"));

            // Select End Date
            _leavePage.ToDateInputField.ClearViaSendKeys();
            _leavePage.ToDateInputField.SendKeys(endDate.ToString("yyyy-dd-MM"));
            _leavePage.ToDateInputField.SendKeys(Keys.Tab);

            _globalHelpers.Wait.Until(d => _leavePage.PartialDaysSelectElement.Displayed);
            _leavePage.PartialDaysSelectElement.Click();
            //_globalHelpers.SelectElementByText(_leavePage.PartialDaysOptions, "All Days");
            _leavePage.PartialDaysOptions.SelectItemByText("All Days");
            _globalHelpers.Wait.Until(d => _leavePage.DurationSelectElement.Displayed);
            _leavePage.DurationSelectElement.Click();
            //_globalHelpers.SelectElementByText(_leavePage.DurationOptions, "Half Day - Morning");
            _leavePage.DurationOptions.SelectItemByText("Half Day - Morning");
            _leavePage.ApplyButton.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);

            // Check that record exists in main Leave List and My Leave list.
            _leavePage.LeaveListLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.LeaveListHeader.Displayed);
            Assert.That(_leavePage.RecordCountSpan.Displayed, Is.True);
            (recordExists, leaveStatus, leaveRecord) = _leavePageHelpers.GetLeaveRecordForDateRange(startDate, endDate);
            Assert.Multiple(() =>
            {
                Assert.That(recordExists, Is.True);
                Assert.That(leaveStatus, Is.EqualTo("Pending"));
            });
            _leavePage.MyLeaveLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.LeaveListHeader.Displayed);
            Assert.That(_leavePage.RecordCountSpan.Displayed, Is.True);
            (recordExists, leaveStatus, leaveRecord) = _leavePageHelpers.GetLeaveRecordForDateRange(startDate, endDate);
            Assert.Multiple(() =>
            {
                Assert.That(recordExists, Is.True);
                Assert.That(leaveStatus, Is.EqualTo("Pending"));
            });

            // Cancel leave and confirm it now shows as cancelled
            _leavePage.CancelLeaveButton.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
            _globalHelpers.Wait.Until(d => _leavePage.LeaveRecords.Count > 0);
            (recordExists, leaveStatus, leaveRecord) = _leavePageHelpers.GetLeaveRecordForDateRange(startDate, endDate);
            Assert.Multiple(() =>
            {
                Assert.That(recordExists, Is.True);
                Assert.That(leaveStatus, Is.EqualTo("Cancelled"));
            });
            Thread.Sleep(5000);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
