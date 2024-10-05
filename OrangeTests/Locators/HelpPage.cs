using OpenQA.Selenium;

namespace OrangeHRMTests.Locators
{
    public class HelpPage
    {
        private readonly IWebDriver _driver;

        public HelpPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string Url => "https://starterhelp.orangehrm.com/hc/en-us";
        public IWebElement AdminUserGuideLink => _driver.FindElement(By.XPath("//a[contains(@href, '/hc/en-us/categories/360002945799-Admin-User-Guide')]"));
        public IWebElement EmployeeUserGuideLink => _driver.FindElement(By.XPath("//a[contains(@href, '/hc/en-us/categories/360002926580-Employee-User-Guide')]"));
        public IWebElement MobileAppLink => _driver.FindElement(By.XPath("//a[contains(@href, '/hc/en-us/categories/360002945899-Mobile-App')]"));
        public IWebElement AWSGuideLink => _driver.FindElement(By.XPath("//a[contains(@href, '/hc/en-us/categories/12036145530140-AWS-Guide')]"));
        public IWebElement FAQsLink => _driver.FindElement(By.XPath("//a[contains(@href, '/hc/en-us/categories/360002856800-FAQs')]"));
        public IWebElement KnowledgeBaseSection => _driver.FindElement(By.XPath("//section[contains(@class, 'knowledge-base')]"));
    }
}
