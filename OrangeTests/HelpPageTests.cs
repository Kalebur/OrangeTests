using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests
{
    public class HelpPageTests
    {
        private IWebDriver _driver;
        private GlobalLocators _globalLocators;
        private GlobalHelpers _globalHelpers;
        private HelpPage _helpPage;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, new Random(), _globalLocators);
            _helpPage = new HelpPage(_driver);
        }

        [Test]
        public void HelpPage_ContainsExpectedLinks()
        {
            _globalHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(_driver => _globalLocators.HelpButton.Displayed);
            _globalLocators.HelpButton.Click();

            Assert.That(_driver.WindowHandles, Has.Count.EqualTo(2));
            _driver.SwitchTo().Window(_driver.WindowHandles[1]);
            Assert.That(_driver.CurrentWindowHandle, Is.EqualTo(_driver.WindowHandles[1]));
            _globalHelpers.Wait.Until(_driver => _helpPage.SearchBar.Displayed);

            Assert.Multiple(() =>
            {
                if (_globalHelpers.GetWindowWidth() < 1000)
                {
                    _helpPage.HamburgerMenuButton.Click();
                    Assert.That(_helpPage.ResponsiveSignInLink.Displayed, Is.True);
                }
                else
                {
                    Assert.That(_helpPage.SignInLink.Displayed, Is.True);
                }
                Assert.That(_helpPage.SearchBar.Displayed, Is.True);
                Assert.That(_helpPage.AdminUserGuideLink.Displayed, Is.True);
                Assert.That(_helpPage.EmployeeUserGuideLink.Displayed, Is.True);
                Assert.That(_helpPage.MobileAppLink.Displayed, Is.True);
                Assert.That(_helpPage.AWSGuideLink.Displayed, Is.True);
                Assert.That(_helpPage.FAQsLink.Displayed, Is.True);
            });
        }

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
