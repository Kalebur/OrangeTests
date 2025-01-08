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

        [SetUp]
        public void Setup()
        {
            Random random = new();
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, random, _globalLocators);
            _adminPage = new AdminPage(_driver, _globalHelpers, _globalLocators, random);
        }

        [Test]
        public void CanSearchForAndEditUsers()
        {
            _adminPage.NavigateToAdminPage();
            Assert.That(_globalLocators.AdminLink.GetAttribute("class"), Does.Contain("active"));

            var username = _adminPage.GetTestUsername();
            Assert.That(username, Is.Not.EqualTo("Admin"));
            _adminPage.SearchForUserByUsername(username);
            Assert.That(_adminPage.GetRecordCount(), Is.EqualTo(1));

            var currentUserData = _adminPage.ParseUserTableRow(_adminPage.Users.First());
            var testUserData = _globalHelpers.GenerateRandomUser();
            _adminPage.EditUser(testUserData);
            _adminPage.SearchForUserByUsername(testUserData.Username);
            Assert.That(_adminPage.GetRecordCount(), Is.EqualTo(1));

            var updatedUserData = _adminPage.ParseUserTableRow(_adminPage.Users.First());
            _adminPage.AssertUpdateWasSuccessful(testUserData, updatedUserData);

            _globalHelpers.DeleteRecord(_adminPage.Users.First());
            _adminPage.SearchButton.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.RecordsTable.Displayed);
            Assert.That(_adminPage.GetRecordCount(), Is.EqualTo(0));
        }

        [Test]
        public void CanAddUser()
        {
            _adminPage.NavigateToAdminPage();
            Assert.That(_globalLocators.AdminLink.GetAttribute("class"), 
                Does.Contain("active"));

            _adminPage.AddUserButton.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => _adminPage.ConfirmPasswordTextBox.Displayed);
            var newUser = _globalHelpers.GenerateRandomUser();
            _adminPage.AddUser(newUser);
            _globalHelpers.Wait.Until(d => _adminPage.SystemUsersDisplayToggleButton.Displayed);
            _globalHelpers.Logout();
            _globalHelpers.LoginWithCredentials(newUser.Username, newUser.Password);
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdown.Displayed);
            Assert.Multiple(() =>
            {
                Assert.That(_globalLocators.DashboardLink.GetAttribute("class"), 
                    Does.Contain("active"));
                Assert.That(_globalLocators.UserName.Text,
                    Is.EqualTo($"{newUser.Employee.FirstName} {newUser.Employee.LastName}"));
            });
        }

        [Test]
        public void CanDeleteUser()
        {
            _adminPage.NavigateToAdminPage();
            Assert.That(_globalLocators.AdminLink.GetAttribute("class"), Does.Contain("active"));

            string username = _adminPage.GetTestUsername();
            _adminPage.SearchForUserByUsername(username);
            Assert.That(_adminPage.GetRecordCount(), Is.EqualTo(1));

            _globalHelpers.DeleteRecord(_adminPage.Users.First());
            _adminPage.UsernameTextBox.ClearViaSendKeys();
            _adminPage.SearchForUserByUsername(username);
            _globalHelpers.Wait.Until(d => _globalLocators.RecordsTable.Displayed);
            Assert.That(_adminPage.GetRecordCount(), Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
