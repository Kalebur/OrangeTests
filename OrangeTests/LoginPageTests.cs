using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OrangeTests.Helpers;
using OrangeTests.Locators;

namespace OrangeTests
{
    public class LoginPageTests
    {
        private IWebDriver _driver;
        private LoginPage _loginPage;
        private LoginHelpers _loginHelpers;
        private GlobalLocators _globalLocators;
        private GlobalHelpers _globalHelpers;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _loginPage = new LoginPage(_driver);
            _loginHelpers = new LoginHelpers(_driver, _loginPage);
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver);
        }

        [Test]
        public void CanLogin()
        {
            _loginHelpers.LoginAs("admin");

            Assert.That(_driver.Url, Is.EqualTo("https://opensource-demo.orangehrmlive.com/dashboard/index"));
        }

        [Test]
        public void CanLogout()
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdown.Displayed);
            _globalLocators.UserDropdown.Click();
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdownMenu.Displayed);
            _globalLocators.LogoutButton.Click();

            Assert.That(_driver.Url, Is.EqualTo(_loginPage.Url));
        }

        [Test]
        public void InvalidLoginCredentials_FailsToLogin_AndProducesCorrectError()
        {
            _loginHelpers.LoginAs("default_user");
            _globalHelpers.Wait.Until(_driver => _loginPage.ErrorMessage.Displayed);

            Assert.That(_driver.Url, Is.EqualTo(_loginPage.Url));
            Assert.That(_loginPage.ErrorMessage.Displayed);
            Assert.That(_loginPage.ErrorMessage.Text, Is.EqualTo(_loginPage.InvalidCredentialsErrorText));
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
