using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Tests
{
    public class AdminPageTests
    {
        private IWebDriver _driver;
        private GlobalHelpers _globalHelpers;
        private GlobalLocators _globalLocators;
        private AdminPage _adminPage;
        private AdminHelpers _adminHelpers;

        [SetUp]
        public void Setup()
        {
            Random random = new();
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, random, _globalLocators);
            _adminPage = new AdminPage(_driver);
            _adminHelpers = new AdminHelpers(_driver, _adminPage, _globalHelpers, _globalLocators, random);
        }

        [Test]
        public void CanSearchForAndEditUsers()
        {
            _adminHelpers.NavigateToAdminPage();
            Assert.That(_globalLocators.AdminLink.GetAttribute("class"), Does.Contain("active"));

            var username = _adminHelpers.GetTestUsername();
            Assert.That(username, Is.Not.EqualTo("Admin"));
            _adminHelpers.SearchForUserByUsername(username);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(1));

            var currentUserData = _adminHelpers.ParseUserTableRow(_adminPage.Users.First());
            var testUserData = _globalHelpers.GenerateRandomUser();
            _adminHelpers.EditUser(testUserData);
            _adminHelpers.SearchForUserByUsername(testUserData.Username);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(1));

            var updatedUserData = _adminHelpers.ParseUserTableRow(_adminPage.Users.First());
            _adminHelpers.AssertUpdateWasSuccessful(testUserData, updatedUserData);

            _globalHelpers.DeleteRecord(_adminPage.Users.First());
            _adminPage.SearchButton.Click();
            _globalHelpers.Wait.Until(d => _adminPage.RecordCountSpan.Displayed);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(0));
        }

        [Test]
        public void CanAddUser()
        {
            _adminHelpers.NavigateToAdminPage();
            Assert.That(_globalLocators.AdminLink.GetAttribute("class"), Does.Contain("active"));

            _adminPage.AddUserButton.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => _adminPage.ConfirmPasswordTextBox.Displayed);
            var newUser = _globalHelpers.GenerateRandomUser();
            _adminHelpers.AddUser(newUser);
            _globalHelpers.Wait.Until(d => _adminPage.SystemUsersDisplayToggleButton.Displayed);
            _globalHelpers.Logout();
            _globalHelpers.LoginWithCredentials(newUser.Username, newUser.Password);
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdown.Displayed);
            Assert.Multiple(() =>
            {
                Assert.That(_globalLocators.DashboardLink.GetAttribute("class"), Does.Contain("active"));
                Assert.That(_globalLocators.UserName.Text,
                    Is.EqualTo(newUser.Employee.FirstName + " " + newUser.Employee.LastName));
            });
        }

        [Test]
        public void CanDeleteUser()
        {
            _adminHelpers.NavigateToAdminPage();
            Assert.That(_globalLocators.AdminLink.GetAttribute("class"), Does.Contain("active"));

            string username = _adminHelpers.GetTestUsername();
            _adminHelpers.SearchForUserByUsername(username);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(1));

            _globalHelpers.DeleteRecord(_adminPage.Users.First());
            _adminPage.UsernameTextBox.ClearViaSendKeys();
            _adminHelpers.SearchForUserByUsername(username);
            _globalHelpers.Wait.Until(d => _adminPage.RecordCountSpan.Displayed);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
