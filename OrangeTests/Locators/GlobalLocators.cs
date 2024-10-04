using OpenQA.Selenium;

namespace OrangeTests.Locators
{
    public class GlobalLocators
    {
        private readonly IWebDriver _driver;

        public GlobalLocators(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement UserDropdown => _driver.FindElement(By.XPath("//li[@class='oxd-userdropdown']"));
        public IWebElement UserDropdownMenu => _driver.FindElement(By.XPath("//ul[@class='oxd-dropdown-menu']"));
        public IWebElement LogoutButton => UserDropdownMenu.FindElement(By.XPath("//a[contains(text(), 'Logout')]"));
    }
}
