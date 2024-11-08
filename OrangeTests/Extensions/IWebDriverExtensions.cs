using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OrangeHRMTests.Extensions
{
    public static class IWebDriverExtensions
    {
        public static void WaitFor(this IWebDriver driver, Func<IWebElement> element)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => element().Displayed);
        }
    }
}
