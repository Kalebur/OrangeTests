using OpenQA.Selenium;

namespace OrangeHRMTests.Locators
{
    public class GlobalLocators(IWebDriver driver)
    {
        private readonly IWebDriver _driver = driver;

        public IWebElement UserDropdown => _driver.FindElement(By.XPath("//li[@class='oxd-userdropdown']"));
        public IWebElement UserName => _driver.FindElement(By.XPath("//p[contains(@class, 'oxd-userdropdown-name')]"));
        public IWebElement UserDropdownMenu => _driver.FindElement(By.XPath("//ul[@class='oxd-dropdown-menu']"));
        public IWebElement LogoutButton => UserDropdownMenu.FindElement(By.XPath("//a[contains(text(), 'Logout')]"));
        public IWebElement AdminLink => _driver.FindElement(By.XPath("//a[contains(@href, '/admin/viewAdminModule')]"));
        public IWebElement LeaveLink => _driver.FindElement(By.XPath("//a[contains(@href, '/leave/viewLeaveModule')]"));
        public IWebElement MyInfoLink => _driver.FindElement(By.XPath("//a[contains(@href, '/pim/viewMyDetails')]"));
        public IWebElement DashboardLink => _driver.FindElement(By.XPath("//a[contains(@href, '/dashboard/index')]"));
        public IWebElement HelpButton => _driver.FindElement(By.XPath("//button[@title='Help']"));
        public IWebElement SuccessAlert => _driver.FindElement(By.XPath("//p[text()='Success']"));
    }
}
