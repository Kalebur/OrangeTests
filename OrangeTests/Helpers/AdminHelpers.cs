using OpenQA.Selenium;
using OrangeHRMTests.Locators;
using System.Text.RegularExpressions;

namespace OrangeHRMTests.Helpers
{
    public class AdminHelpers
    {
        private readonly IWebDriver _driver;
        private readonly AdminPage _adminPage;
        private readonly GlobalHelpers _globalHelpers;

        public AdminHelpers(IWebDriver driver, AdminPage adminPage, GlobalHelpers globalHelpers)
        {
            _driver = driver;
            _adminPage = adminPage;
            _globalHelpers = globalHelpers;
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
