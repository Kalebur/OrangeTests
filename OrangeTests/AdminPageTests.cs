using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;
using OrangeHRMTests.Models;

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
            _adminHelpers.SearchForUserByUsername("DingleChingle");
            Assert.That(_adminHelpers
                .GetRecordCount(), Is.EqualTo(0) | Is.EqualTo(1));
        }

        [Test]
        public void CanEditUser()
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.AdminLink.Displayed);
            _globalLocators.AdminLink.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => _adminPage.SystemUsersDisplayToggleButton.Displayed);
            Assert.That(_driver.Url, Is.EqualTo(_adminPage.Url));

            var username = _adminHelpers.GetTestUsername();
            _adminHelpers.SearchForUserByUsername(username);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(1));

            var currentUserData = _adminHelpers.ParseUserTableRow(_adminPage.Users.First());
            var newUserData = _adminHelpers.GenerateRandomUser();
            _adminHelpers.EditUser(newUserData);
            _adminHelpers.SearchForUserByUsername(newUserData.Username);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(1));

            var updatedUserData = _adminHelpers.ParseUserTableRow(_adminPage.Users.First());
            Assert.Multiple(() =>
            {
                Assert.That(updatedUserData.Username, Is.EqualTo(newUserData.Username));
                Assert.That(updatedUserData.Employee.FirstName, Is.EqualTo(newUserData.Employee.FirstName));
                Assert.That(updatedUserData.Employee.LastName, Is.EqualTo(newUserData.Employee.LastName));
                Assert.That(updatedUserData.UserRole, Is.EqualTo(newUserData.UserRole));
                Assert.That(updatedUserData.IsEnabled, Is.EqualTo(newUserData.IsEnabled));
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
