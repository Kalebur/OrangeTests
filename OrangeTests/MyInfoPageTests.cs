using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests
{
    public class MyInfoPageTests
    {
        private IWebDriver _driver;
        private GlobalHelpers _globalHelpers;
        private GlobalLocators _globalLocators;
        private LoginHelpers _loginHelpers;
        private MyInfoPage _myInfoPage;
        private MyInfoPageHelpers _myInfoPageHelpers;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalHelpers = new GlobalHelpers(_driver, new Random());
            _globalLocators = new GlobalLocators(_driver);
            _loginHelpers = new LoginHelpers(_driver, new LoginPage(_driver), _globalHelpers, _globalLocators);
            _myInfoPage = new MyInfoPage(_driver);
            _myInfoPageHelpers = new MyInfoPageHelpers(_driver, _myInfoPage, _globalHelpers);
        }

        [Test]
        public void CanEditOwnName()
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.MyInfoLink.Displayed);
            _globalLocators.MyInfoLink.Click();
            _globalHelpers.Wait.Until(d => _myInfoPage.PersonalDetailsTabButton.Displayed);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
