using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OrangeTests.Helpers
{
    public class GlobalHelpers
    {
        private readonly IWebDriver _driver;

        public GlobalHelpers(IWebDriver driver)
        {
            _driver = driver;
        }

        public WebDriverWait Wait => new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }
}
