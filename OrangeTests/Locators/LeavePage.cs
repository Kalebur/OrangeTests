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

        public string InsufficientBalanceErrorText = "Balance not sufficient";

        public IWebElement ApplyLink => _driver.FindElement(By.XPath("//nav//a[contains(text(), 'Apply')]"));
        public IWebElement ApplyButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement LeaveBalanceField => _driver.FindElement(By.XPath("//p[contains(@class, 'orangehrm-leave-balance-text')]"));
        public IWebElement LeaveTypeSelectElement => _driver.FindElement(By.XPath("//label[contains(text(), 'Leave Type')]//parent::div//following-sibling::div"));
        public IList<IWebElement> LeaveTypeOptions => _driver.FindElements(By.XPath("//label[contains(text(), 'Leave Type')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement FromDateInputField => _driver.FindElement(By.XPath("//label[contains(text(), 'From Date')]//parent::div//following-sibling::div//input"));
        public IWebElement ToDateInputField => _driver.FindElement(By.XPath("//label[contains(text(), 'To Date')]//parent::div//following-sibling::div//input"));
        public IWebElement CalendarDropdown => _driver.FindElement(By.XPath("//label[contains(text(), 'To Date')]//parent::div//following-sibling::div//input"));
        public IWebElement MonthSelector => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-calendar-selector-month')]"));
        public IWebElement YearSelector => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-calendar-selector-year')]"));
        public IList<IWebElement> Months => _driver.FindElements(By.XPath("//li[contains(@class, 'oxd-calendar-dropdown')]")).ToList();
        public IList<IWebElement> Years => _driver.FindElements(By.XPath("//li[contains(@class, 'oxd-calendar-dropdown')]")).ToList();
        public IList<IWebElement> Dates => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-calendar-dates-grid')]//div[contains(@class, 'oxd-calendar-date') and not(contains(@class, '-wrapper'))]")).ToList();
        public IWebElement PartialDaysSelectElement => _driver.FindElement(By.XPath("//label[contains(text(), 'Partial Days')]//parent::div//following-sibling::div"));
        public IList<IWebElement> PartialDaysOptions => _driver.FindElements(By.XPath("//label[contains(text(), 'Partial Days')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement DurationSelectElement => _driver.FindElement(By.XPath("//label[contains(text(), 'Duration')]//parent::div//following-sibling::div"));
        public IList<IWebElement> DurationOptions => _driver.FindElements(By.XPath("//label[contains(text(), 'Duration')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement CommentsTextArea => _driver.FindElement(By.XPath("//textarea"));






    }
}
