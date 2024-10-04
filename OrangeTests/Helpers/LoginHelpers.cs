using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OrangeTests.Locators;

namespace OrangeTests.Helpers
{
    public class LoginHelpers
    {
        private readonly LoginPage _loginPage;
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, (string userName, string Password)> _users = new()
        {
            { "admin", ("Admin", "admin123") },
        };
        private WebDriverWait _wait;

        public LoginHelpers(IWebDriver driver, LoginPage loginPage)
        {
            _driver = driver;
            _loginPage = loginPage;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void LoginAs(string user)
        {
            string userName = string.Empty;
            string password = string.Empty;

            _driver.Navigate().GoToUrl(_loginPage.Url);
            _wait.Until(_driver => _loginPage.UsernameTextBox.Displayed);

            if (_users.ContainsKey(user))
            {
                userName = _users[user].userName;
                password = _users[user].Password;
            }
            else
            {
                userName = "InvalidUser";
                password = "IncorrectPassword";
            }

            _loginPage.UsernameTextBox.SendKeys(userName);
            _loginPage.PasswordTextBox.SendKeys(password);
            _loginPage.LoginButton.Click();
        }
    }
}
