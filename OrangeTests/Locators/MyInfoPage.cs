using OpenQA.Selenium;

namespace OrangeHRMTests.Locators
{
    public class MyInfoPage(IWebDriver driver)
    {
        private readonly IWebDriver _driver = driver;

        public IWebElement PersonalDetailsTabButton => _driver.FindElement(By.XPath("//div[@role='tab']//a[contains(text(), 'Personal Details')]"));
        public IWebElement FirstNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-firstname')]"));
        public IWebElement MiddleNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-middlename')]"));
        public IWebElement LastNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-lastname')]"));
        public IWebElement SavePersonalDetailsButton => _driver.FindElement(By.XPath("//h6[text()='Personal Details']//following-sibling::form//button[@type='submit']"));
        public IWebElement RecordCountSpan => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-attachment')]//hr[contains(@class, 'oxd-divider orangehrm-horizontal-margin')]//following-sibling::div//span"));
        public IWebElement FileInput => _driver.FindElement(By.XPath("//input[@type='file']"));
        public IWebElement FilenameDiv => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-file-input-div')]"));
        public IWebElement AddAttachmentButton => _driver.FindElement(By.XPath("//button//i[contains(@class, 'bi-plus')]//parent::button"));
        public IList<IWebElement> Attachments => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-table-body')]/child::div[contains(@class, 'oxd-table-card')]"));
        public IWebElement SaveAttachmentButton => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-attachment')]//button[@type='submit']"));
        public IWebElement ResponsiveAttachmentFilename(IWebElement element) => element.FindElement(By.XPath(".//div[contains(@class, 'card-item card-header-slot-content --left')]//div[contains(@class, 'data')]"));


    }
}
