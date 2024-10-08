using OpenQA.Selenium;

namespace OrangeHRMTests.Locators
{
    public class LeavePage
    {
        private readonly IWebDriver _driver;

        public LeavePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement ApplyLink => _driver.FindElement(By.XPath("//nav//a[contains(text(), 'Apply')]"));
        public IWebElement ApplyButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement LeaveBalanceField => _driver.FindElement(By.XPath("//p[contains(@class, 'orangehrm-leave-balance-text')]"));
        public IWebElement LeaveTypeSelectElement => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-select-wrapper')]"));
        public IList<IWebElement> LeaveTypeOptions => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement FromDateInputField => _driver.FindElement(By.XPath("//label[contains(text(), 'From Date')]//parent::div//following-sibling::div//input"));
        public IWebElement ToDateInputField => _driver.FindElement(By.XPath("//label[contains(text(), 'To Date')]//parent::div//following-sibling::div//input"));
        public IWebElement CalendarDropdown => _driver.FindElement(By.XPath("//label[contains(text(), 'To Date')]//parent::div//following-sibling::div//input"));





    }
}
