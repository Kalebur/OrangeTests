using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Extensions;
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

        [TestCase("John", "William", "Waterhouse", "John Waterhouse")]
        [TestCase("Jane", "Marie", "McDougal", "Jane McDougal")]
        [TestCase("Trini", "", "Quan", "Trini Quan")]
        [TestCase("Maxwell", "Hunter", "Sloan", "Maxwell Sloan")]
        [TestCase("Selena", "Ann", "Shepherd", "Selena Shepherd")]
        public void CanEditOwnName(string firstName, string middleName, string lastName, string expectedName)
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.MyInfoLink.Displayed);
            _globalLocators.MyInfoLink.Click();
            _globalHelpers.Wait.Until(d => _myInfoPage.PersonalDetailsTabButton.Displayed);

            _myInfoPage.FirstNameTextBox.ClearViaSendKeys();
            _myInfoPage.FirstNameTextBox.SendKeys(firstName);
            Assert.That(_myInfoPage.FirstNameTextBox.GetAttribute("value"), Is.EqualTo(firstName));
            _myInfoPage.MiddleNameTextBox.ClearViaSendKeys();
            _myInfoPage.MiddleNameTextBox.SendKeys(middleName);
            Assert.That(_myInfoPage.MiddleNameTextBox.GetAttribute("value"), Is.EqualTo(middleName));
            _myInfoPage.LastNameTextBox.ClearViaSendKeys();
            _myInfoPage.LastNameTextBox.SendKeys(lastName);
            Assert.That(_myInfoPage.LastNameTextBox.GetAttribute("value"), Is.EqualTo(lastName));

            _myInfoPage.SavePersonalDetailsButton.Click();
            _globalHelpers.Wait.Until(d => _myInfoPage.SuccessAlert.Displayed);
            _globalLocators.DashboardLink.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);
            Assert.That(_globalLocators.UserDropdown.Text, Is.EqualTo(expectedName));
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
