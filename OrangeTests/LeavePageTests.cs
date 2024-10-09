using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
            _globalHelpers = new GlobalHelpers(_driver, new Random());
            _globalLocators = new GlobalLocators(_driver);
            _loginHelpers = new LoginHelpers(_driver, new LoginPage(_driver), _globalHelpers, _globalLocators);
            _leavePage = new LeavePage(_driver);
            _leavePageHelpers = new LeavePageHelpers(_leavePage, _globalHelpers);
        }

        [Test]
        public void CanApplyForLeave()
        {
            var startDate = new DateTime(2024, 11, 22);
            var endDate = new DateTime(2024, 11, 25);

            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);
            _globalLocators.LeaveLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyLink.Displayed);
            _leavePage.ApplyLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyButton.Displayed);
            _leavePage.LeaveTypeSelectElement.Click();
            _leavePageHelpers.SelectRandomLeaveType();

            // Select Start Date
            _leavePage.FromDateInputField.Click();
            _globalHelpers.Wait.Until(d => _leavePage.CalendarDropdown.Displayed);
            Assert.Multiple(() =>
            {
                Assert.That(_leavePage.MonthSelector.Displayed, Is.True);
                Assert.That(_leavePage.YearSelector.Displayed, Is.True);
            });
            _leavePageHelpers.SelectDate(startDate);

            // Select End Date
            _leavePage.ToDateInputField.Click();
            _globalHelpers.Wait.Until(d => _leavePage.CalendarDropdown.Displayed);
            Assert.Multiple(() =>
            {
                Assert.That(_leavePage.MonthSelector.Displayed, Is.True);
                Assert.That(_leavePage.YearSelector.Displayed, Is.True);
            });
            _leavePageHelpers.SelectDate(endDate);

            _leavePage.PartialDaysSelectElement.Click();
            _globalHelpers.SelectElementByText(_leavePage.PartialDaysOptions, "All Days");
            _globalHelpers.Wait.Until(d => _leavePage.DurationSelectElement.Displayed);
            _leavePage.DurationSelectElement.Click();
            _globalHelpers.SelectElementByText(_leavePage.DurationOptions, "Half Day - Morning");
            _leavePage.ApplyButton.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
            _leavePage.MyLeaveLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.MyLeaveListHeader.Displayed);
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
