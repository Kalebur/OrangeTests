using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace OrangeHRMTests.Helpers
{
    public class AdminHelpers
    {
        private readonly IWebDriver _driver;

        public AdminHelpers(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement GetDeleteUserButton(IWebElement UserRow)
        {
            return UserRow.FindElement(By.XPath("//i[contains(@class, 'bi-trash')]/parent::button"));
        }

        public int GetRecordCount(string recordString)
        {
            string pattern = @"\((\d+)\)";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(recordString);

            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }

            return 0;
        }
    }
}
