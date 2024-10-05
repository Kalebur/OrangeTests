using OpenQA.Selenium;

namespace OrangeHRMTests.Locators
{
    public class AdminPage
    {
        private readonly IWebDriver _driver;

        public AdminPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string Url => "https://opensource-demo.orangehrmlive.com/admin/viewSystemUsers";

        public IWebElement SystemUsersDisplayToggleButton => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-table-filter-header-options')]//button"));
        public IWebElement UsernameTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Username')]//parent::div//following-sibling::div//input"));
        public IWebElement UserRoleDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'User Role')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-text--active')]"));
        public IWebElement UserRoleDropdownOptions => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'User Role')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]"));
        public IWebElement EmployeeNameTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Employee Name')]//parent::div//following-sibling::div//input"));
        public IWebElement StatusDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Status')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-text--active')]"));
        public IWebElement StatusDropdownOptions => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Status')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]"));
        public IWebElement SearchButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement AddUserButton => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-paper-container')]//button[contains(@class, 'oxd-button')]"));
        public IWebElement RecordCountSpan => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-horizontal-padding')]//span"));
        public IList<IWebElement> Users => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-table-body')]/child::div[contains(@class, 'oxd-table-card')]"));

    }
}
