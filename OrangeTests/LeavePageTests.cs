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

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalHelpers = new GlobalHelpers(_driver, new Random());
            _globalLocators = new GlobalLocators(_driver);
            _loginHelpers = new LoginHelpers(_driver, new LoginPage(_driver), _globalHelpers, _globalLocators);
            _leavePage = new LeavePage(_driver);
        }

        [Test]
        public void Test1()
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);
            _globalLocators.LeaveLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyLink.Displayed);
            _leavePage.ApplyLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyButton.Displayed);
            _leavePage.LeaveTypeSelectElement.Click();
            foreach (var option in _leavePage.LeaveTypeOptions)
            {
                Console.WriteLine(option.Text);
            }

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
