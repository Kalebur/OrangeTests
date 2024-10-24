using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Helpers
{
    public class LoginHelpers
    {
        private readonly LoginPage _loginPage;
        private readonly GlobalHelpers _globalHelpers;
        private readonly GlobalLocators _globalLocators;
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, (string userName, string Password)> _users = new()
        {
            { "admin", ("Admin", "admin123") },
        };
        private readonly WebDriverWait _wait;

        public LoginHelpers(IWebDriver driver, LoginPage loginPage, GlobalHelpers globalHelpers, GlobalLocators globalLocators)
        {
            _driver = driver;
            _loginPage = loginPage;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _globalHelpers = globalHelpers;
            _globalLocators = globalLocators;

        }

        public void LoginAs(string user, bool maximizeWindow = false)
        {
            string username = string.Empty;
            string password = string.Empty;

            _driver.Navigate().GoToUrl(_loginPage.Url);
            if (maximizeWindow) _driver.Manage().Window.Maximize();
            _wait.Until(_driver => _loginPage.UsernameTextBox.Displayed);

            if (_users.TryGetValue(user, out (string username, string Password) value))
            {
                username = value.username;
                password = value.Password;
            }
            else
            {
                username = "InvalidUser";
                password = "IncorrectPassword";
            }

            _loginPage.UsernameTextBox.SendKeys(username);
            _loginPage.PasswordTextBox.SendKeys(password);
            _loginPage.LoginButton.Click();
        }

        public void LoginWithCredentials(string username, string password, bool maximizeWindow = false)
        {
            _driver.Navigate().GoToUrl(_loginPage.Url);
            _wait.Until(_driver => _loginPage.UsernameTextBox.Displayed);
            _loginPage.UsernameTextBox.SendKeys(username);
            _loginPage.PasswordTextBox.SendKeys(password);
            _loginPage.LoginButton.Click();
        }

        public void Logout()
        {
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdown.Displayed);
            _globalLocators.UserDropdown.Click();
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdownMenu.Displayed);
            _globalLocators.LogoutButton.Click();
            _wait.Until(_driver => _loginPage.UsernameTextBox.Displayed);
        }
    }
}
