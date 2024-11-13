using OpenQA.Selenium;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Models;

namespace OrangeHRMTests.Locators
{
    public class LoginPage(IWebDriver driver, GlobalHelpers globalHelpers, GlobalLocators globalLocators)
    {
        private readonly IWebDriver _driver = driver;
        private readonly GlobalHelpers _globalHelpers = globalHelpers;
        private readonly GlobalLocators _globalLocators = globalLocators;
        private readonly Dictionary<string, (string userName, string Password)> _users = new()
        {
            { "admin", ("Admin", "admin123") },
        };

        public string Url => "https://opensource-demo.orangehrmlive.com/";
        public string InvalidCredentialsErrorText => "Invalid credentials";
        public IWebElement UsernameTextBox => _driver.FindElement(By.XPath("//input[@name='username']"));
        public IWebElement PasswordTextBox => _driver.FindElement(By.XPath("//input[@type='password']"));
        public IWebElement LoginButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement ErrorMessage => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-alert-content--error')]"));

        public void LoginAs(string user, bool maximizeWindow = false)
        {
            string username = string.Empty;
            string password = string.Empty;

            _driver.Navigate().GoToUrl(Url);
            if (maximizeWindow) _driver.Manage().Window.Maximize();
            _globalHelpers.Wait.Until(_driver => UsernameTextBox.Displayed);

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

            UsernameTextBox.SendKeys(username);
            PasswordTextBox.SendKeys(password);
            LoginButton.Click();
        }

        public void LoginWithCredentials(string username, string password, bool maximizeWindow = false)
        {
            _driver.Navigate().GoToUrl(Url);
            _globalHelpers.Wait.Until(_driver => UsernameTextBox.Displayed);
            UsernameTextBox.SendKeys(username);
            PasswordTextBox.SendKeys(password);
            LoginButton.Click();
        }

        public void Logout()
        {
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdown.Displayed);
            _globalLocators.UserDropdown.Click();
            _globalHelpers.Wait.Until(_driver => _globalLocators.UserDropdownMenu.Displayed);
            _globalLocators.LogoutButton.Click();
            _globalHelpers.Wait.Until(_driver => UsernameTextBox.Displayed);
        }
    }
}
