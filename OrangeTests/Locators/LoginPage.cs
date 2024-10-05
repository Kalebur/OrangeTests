using OpenQA.Selenium;

namespace OrangeHRMTests.Locators
{
    public class LoginPage(IWebDriver driver)
    {
        private readonly IWebDriver _driver = driver;

        public string Url => "https://opensource-demo.orangehrmlive.com/auth/login";
        public string InvalidCredentialsErrorText => "Invalid credentials";
        public IWebElement UsernameTextBox => _driver.FindElement(By.XPath("//input[@name='username']"));
        public IWebElement PasswordTextBox => _driver.FindElement(By.XPath("//input[@type='password']"));
        public IWebElement LoginButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement ErrorMessage => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-alert-content--error')]"));

        
    }
}
