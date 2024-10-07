using OpenQA.Selenium;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Helpers
{
    public class MyInfoPageHelpers
    {
        private readonly IWebDriver _driver;
        private readonly MyInfoPage _myInfoPage;
        private readonly GlobalHelpers _globalHelpers;

        public MyInfoPageHelpers(IWebDriver driver, MyInfoPage myInfoPage, GlobalHelpers globalHelpers)
        {
            _driver = driver;
            _myInfoPage = myInfoPage;
            _globalHelpers = globalHelpers;
        }
    }
}
