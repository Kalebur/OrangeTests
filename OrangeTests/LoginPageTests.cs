using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests
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
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver);
            _loginHelpers = new LoginHelpers(_driver, _loginPage, _globalHelpers, _globalLocators);
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
            _loginHelpers.Logout();

            Assert.That(_driver.Url, Is.EqualTo(_loginPage.Url));
        }

        [Test]
        public void InvalidLoginCredentials_FailsToLogin_AndProducesCorrectError()
        {
            _loginHelpers.LoginAs("default_user");
            _globalHelpers.Wait.Until(_driver => _loginPage.ErrorMessage.Displayed);

            Assert.Multiple(() =>
            {
                Assert.That(_driver.Url, Is.EqualTo(_loginPage.Url));
                Assert.That(_loginPage.ErrorMessage.Displayed);
                Assert.That(_loginPage.ErrorMessage.Text, Is.EqualTo(_loginPage.InvalidCredentialsErrorText));
            });
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
