using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Helpers
{
    public class GlobalHelpers(IWebDriver driver)
    {
        private readonly IWebDriver _driver = driver;
        private readonly Actions _actions = new Actions(driver);

        public WebDriverWait Wait => new(_driver, TimeSpan.FromSeconds(10));

        public bool IsElementPresentOnPage(IWebElement element)
        {
            try
            {
                if (element.Displayed) return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return false;
        }

        public void ClickViaActions(IWebElement element)
        {
            _actions.ScrollToElement(element);
            _actions.Click();
        }

        public Int64 GetWindowWidth()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            var width = (Int64)js.ExecuteScript("return document.documentElement.clientWidth;");
            return width;
        }

    }

}
