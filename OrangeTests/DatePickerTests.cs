using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace OrangeTests
{
    public class Tests
    {
        private IWebDriver _driver;
        private DatePickerPage _datePickerPage;
        private WebDriverWait _wait;


        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _datePickerPage = new DatePickerPage(_driver);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        }

        //[TestCase("8/15/2082")]
        //[TestCase("5/23/2003")]
        //[TestCase("10/27/2016")]
        //[TestCase("3/15/1492")]
        [TestCase("10/31/5284")]
        public void SelectDate(string date)
        {
            _driver.Navigate().GoToUrl(_datePickerPage.Url);
            _wait.Until(d => (IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete");
            Assert.That(_datePickerPage.Calendar.Displayed, Is.True);
            Assert.That(_datePickerPage.CalendarNextButton.Displayed, Is.True);
            Assert.That(_datePickerPage.CalendarPreviousButton.Displayed, Is.True);

            _datePickerPage.SelectDate(date);

            //Assert.That(_datePickerPage.TargetDateIsSelected(date), Is.True);
        }

        [TearDown] 
        public void Teardown() {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}