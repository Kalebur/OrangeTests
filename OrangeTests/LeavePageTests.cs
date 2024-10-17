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
            var startDate = new DateTime(2024, 12, 3);
            var endDate = new DateTime(2024, 12, 5);
            (bool recordExists, string leaveStatus) = (false, null);

            _loginHelpers.LoginAs("admin", true);
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);
            _globalLocators.LeaveLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyLink.Displayed);
            _leavePage.ApplyLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyButton.Displayed);
            _leavePage.LeaveTypeSelectElement.Click();
            _leavePageHelpers.SelectRandomLeaveType();

            // Select Start Date
            _leavePage.FromDateInputField.SendKeys(startDate.ToString("yyyy-MM-dd"));

            // Select End Date
            _leavePage.ToDateInputField.ClearViaSendKeys();
            _leavePage.ToDateInputField.SendKeys(endDate.ToString("yyyy-MM-dd"));
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
            _leavePage.LeaveListLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.LeaveListHeader.Displayed);
            Assert.That(_leavePage.RecordCountSpan.Displayed, Is.True);
            (recordExists, leaveStatus) = _leavePageHelpers.GetLeaveRecordForDateRange(startDate, endDate);
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
