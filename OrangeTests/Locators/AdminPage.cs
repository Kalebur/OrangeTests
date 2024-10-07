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

        public string Url => "https://opensource-demo.orangehrmlive.com/web/index.php/admin/viewSystemUsers";

        public IWebElement SystemUsersDisplayToggleButton => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-table-filter-header-options')]//button"));
        public IWebElement UsernameTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Username')]//parent::div//following-sibling::div//input"));
        public IWebElement UserRoleDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'User Role')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-text--active')]"));
        public IList<IWebElement> UserRoleDropdownOptions => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'User Role')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement EmployeeNameTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Employee Name')]//parent::div//following-sibling::div//input"));
        public IWebElement StatusDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Status')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-text--active')]"));
        public IList<IWebElement> StatusDropdownOptions => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Status')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement SearchButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement AddUserButton => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-paper-container')]//button[contains(@class, 'oxd-button')]"));
        public IWebElement RecordCountSpan => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-horizontal-padding')]//span"));
        public IList<IWebElement> Users => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-table-body')]/child::div[contains(@class, 'oxd-table-card')]"));
        public IWebElement EditEmployeeNameTextBox => _driver.FindElement(By.XPath("//input[@placeholder='Type for hints...']"));
        public IWebElement ChangePasswordCheckBox => _driver.FindElement(By.XPath("//i[contains(@class, 'bi-check')]"));
        public IWebElement PasswordTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'user-password-cell')]//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Password')]//parent::div//following-sibling::div//input"));
        public IWebElement ConfirmPasswordTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'user-password-cell')]/following-sibling::div//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Password')]//parent::div//following-sibling::div//input"));
        public IWebElement CancelButton => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-form-actions')]//button[@type='button']"));
        public IWebElement SaveUserButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement EmployeeAutoCompleteDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-autocomplete-dropdown')]"));
        public IWebElement ResponsiveEmployeeName => _driver.FindElement(By.XPath("//div[contains(text(), 'Employee Name')]//following-sibling::div"));
        public IList<IWebElement> ResponsiveUserFields => _driver.FindElements(By.XPath("//div[contains(@class, 'card-body-slot')]//div[@role='cell']"));
        public IWebElement ResponsiveUsername => _driver.FindElement(By.XPath("//div[contains(text(), 'Username')]//following-sibling::div"));
        public IWebElement ResponsiveUserRole => _driver.FindElement(By.XPath("//div[contains(text(), 'User Role')]//following-sibling::div"));
        public IWebElement ResponsiveUserStatus => _driver.FindElement(By.XPath("//div[contains(text(), 'Status')]//following-sibling::div"));
        public IList<IWebElement> EmployeeNames => EmployeeAutoCompleteDropdown.FindElements(By.XPath("//div[contains(@class, 'autocomplete-option')]/span"));
        public IWebElement FirstAutoCompleteName => _driver.FindElement(By.XPath("//div[contains(@class, 'autocomplete-option')]/span"));
        public IWebElement ModalDeleteButton => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-modal-footer')]//button//i//parent::button"));



    }
}
