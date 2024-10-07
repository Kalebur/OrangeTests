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
            _globalLocators.AdminLink.Click();
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

            _adminHelpers.GetDeleteUserButton(_adminPage.Users.First()).Click();
            _globalHelpers.Wait.Until(d => _adminPage.ModalDeleteButton.Displayed);
            _adminPage.ModalDeleteButton.Click();
            _adminPage.UsernameTextBox.SendKeys(updatedUserData.Username);
            _adminPage.SearchButton.Click();
            _globalHelpers.Wait.Until(d => _adminPage.RecordCountSpan.Displayed);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(0));
        }

        [Test]
        public void CanAddUser()
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.AdminLink.Displayed);
            _globalLocators.AdminLink.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => _adminPage.SystemUsersDisplayToggleButton.Displayed);
            Assert.That(_driver.Url, Is.EqualTo(_adminPage.Url));

            _adminPage.AddUserButton.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => _adminPage.ConfirmPasswordTextBox.Displayed);
            var newUser = _adminHelpers.GenerateRandomUser();
            newUser.IsEnabled = true;
            _adminHelpers.SelectUserRole(newUser.UserRole.ToString());
            var name = _adminHelpers.GetEmployeeNameElement();
            _adminHelpers.SetEmployeeNameFromField(newUser.Employee, name);
            name.Click();
            _adminHelpers.SelectUserStatus(newUser.IsEnabled);
            _adminPage.UsernameTextBox.SendKeys(newUser.Username);
            _adminPage.PasswordTextBox.SendKeys(newUser.Password);
            _adminPage.ConfirmPasswordTextBox.SendKeys(newUser.Password);
            _adminPage.SaveUserButton.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => _adminPage.SystemUsersDisplayToggleButton.Displayed);
            _loginHelpers.Logout();
            _loginHelpers.LoginWithCredentials(newUser.Username, newUser.Password);
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdown.Displayed);
            Assert.That(_driver.Url, Is.EqualTo("https://opensource-demo.orangehrmlive.com/dashboard/index"));
            Assert.That(_globalLocators.UserName.Text,
                Is.EqualTo(newUser.Employee.FirstName + " " + newUser.Employee.LastName));
        }

        [Test]
        public void CanDeleteUser()
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.AdminLink.Displayed);
            _globalLocators.AdminLink.Click();
            _globalHelpers.Wait.Until(d => _adminPage.SystemUsersDisplayToggleButton.Displayed);
            Assert.That(_driver.Url, Is.EqualTo(_adminPage.Url));

            string username;
            do
            {
                username = _adminHelpers.GetTestUsername();
            } while (username == "Admin");

            _adminHelpers.SearchForUserByUsername(username);
            Assert.That(_adminHelpers.GetRecordCount(), Is.EqualTo(1));

            _adminHelpers.GetDeleteUserButton(_adminPage.Users.First()).Click();
            _globalHelpers.Wait.Until(d => _adminPage.ModalDeleteButton.Displayed);
            _adminPage.ModalDeleteButton.Click();

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
