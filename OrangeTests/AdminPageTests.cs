using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests
{
    public class AdminPageTests
    {
        private IWebDriver _driver;
        private GlobalHelpers _globalHelpers;
        private GlobalLocators _globalLocators;
        private LoginHelpers _loginHelpers;
        private AdminPage _adminPage;
        private AdminHelpers _adminHelpers;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalHelpers = new GlobalHelpers(_driver);
            _globalLocators = new GlobalLocators(_driver);
            _loginHelpers = new LoginHelpers(_driver, new LoginPage(_driver), _globalHelpers, _globalLocators);
            _adminPage = new AdminPage(_driver);
            _adminHelpers = new AdminHelpers(_driver, _adminPage, _globalHelpers);
        }

        [Test]
        public void CanSearchForUser()
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.AdminLink.Displayed);
            _globalLocators.AdminLink.Click();
            _globalHelpers.Wait.Until(d => _adminPage.SystemUsersDisplayToggleButton.Displayed);
            Assert.That(_driver.Url, Is.EqualTo(_adminPage.Url));

            if (!_globalHelpers.IsElementPresentOnPage(_adminPage.UsernameTextBox))
            {
                _adminPage.SystemUsersDisplayToggleButton.Click();
            }

            _globalHelpers.Wait.Until(d => _adminPage.UsernameTextBox.Displayed);
            _adminPage.UsernameTextBox.SendKeys("admin");
            _adminPage.SearchButton.Click();
            _globalHelpers.Wait.Until(d => _adminPage.RecordCountSpan.Displayed);

            Assert.That(_adminHelpers.GetRecordCount(_adminPage.RecordCountSpan.Text), Is.EqualTo(0) | Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
