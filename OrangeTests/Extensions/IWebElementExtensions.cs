using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Reflection;

namespace OrangeHRMTests.Extensions
{
    public static class IWebElementExtensions
    {
        private static IWebDriver? _cachedDriver = null;

        public static void ClickViaJavaScript(this IWebElement element)
        {
            if (_cachedDriver is null)
            {
                var elementDriver = (element.GetType().GetProperty("WrappedDriver")?.GetValue(element)) ?? throw new Exception($"{nameof(element)} does not have a web driver.");
                _cachedDriver = elementDriver as IWebDriver;
            }

            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)_cachedDriver!;
            javaScriptExecutor.ExecuteScript("arguments[0].click();", element);
        }

        public static void SelectRandomItem(this IList<IWebElement> elements, Random random)
        {
            elements[random.Next(elements.Count)].Click();
        }

        public static void SelectItemByText(this IList<IWebElement> elements, string text)
        {
            elements.Where(element => element.Text == text).First().Click();
        }

        public static IWebElement GetRandomItem(this IList<IWebElement> elements, Random random)
        {
            return elements[random.Next(elements.Count)];
        }

        public static void ClearViaSendKeys(this IWebElement element)
        {
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
        }
    }
}
