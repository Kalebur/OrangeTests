using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OrangeHRMTests.Helpers
{
    public class GlobalHelpers(IWebDriver driver)
    {
        private readonly IWebDriver _driver = driver;

        public WebDriverWait Wait => new(_driver, TimeSpan.FromSeconds(10));
    }
}
