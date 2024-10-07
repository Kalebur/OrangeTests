using OpenQA.Selenium;

namespace OrangeHRMTests.Locators
{
    public class MyInfoPage
    {
        private readonly IWebDriver _driver;

        public MyInfoPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement PersonalDetailsTabButton => _driver.FindElement(By.XPath("//div[@role='tab']//a[contains(text(), 'Personal Details')]"));
        public IWebElement FirstNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-firstname')]"));
        public IWebElement MiddleNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-middlename')]"));
        public IWebElement LastNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-lastname')]"));
        public IWebElement SavePersonalDetailsButton => _driver.FindElement(By.XPath("//h6[text()='Personal Details']//following-sibling::form//button[@type='submit']"));
        public IWebElement FileInput => _driver.FindElement(By.XPath("//input[@type='file']"));
        public IWebElement AddAttachmentButton => _driver.FindElement(By.XPath("//button//i[contains(@class, 'bi-plus')]//parent::button"));
        public IList<IWebElement> Attachments => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-table-body')]/child::div[contains(@class, 'oxd-table-card')]"));
        public IWebElement SuccessAlert => _driver.FindElement(By.XPath("//p[text()='Successfully Updated']"));


    }
}
