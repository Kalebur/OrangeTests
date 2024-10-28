using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests
{
    public class MyInfoPageTests
    {
        private IWebDriver _driver;
        private GlobalHelpers _globalHelpers;
        private GlobalLocators _globalLocators;
        private MyInfoPage _myInfoPage;
        private MyInfoPageHelpers _myInfoPageHelpers;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, new Random(), _globalLocators);
            _myInfoPage = new MyInfoPage(_driver);
            _myInfoPageHelpers = new MyInfoPageHelpers(_driver, _myInfoPage, _globalHelpers, _globalLocators);
        }

        //[TestCase("John", "William", "Waterhouse", "John Waterhouse")]
        //[TestCase("Jane", "Marie", "McDougal", "Jane McDougal")]
        [TestCase("Trini", "", "Quan", "Trini Quan")]
        //[TestCase("Maxwell", "Hunter", "Sloan", "Maxwell Sloan")]
        //[TestCase("Selena", "Ann", "Shepherd", "Selena Shepherd")]
        public void CanEditOwnName(string firstName, string middleName, string lastName, string expectedName)
        {
            _myInfoPageHelpers.NavigateTo();

            _myInfoPage.FirstNameTextBox.ClearViaSendKeys();
            _myInfoPage.FirstNameTextBox.SendKeys(firstName);
            _myInfoPage.MiddleNameTextBox.ClearViaSendKeys();
            _myInfoPage.MiddleNameTextBox.SendKeys(middleName);
            _myInfoPage.LastNameTextBox.ClearViaSendKeys();
            _myInfoPage.LastNameTextBox.SendKeys(lastName);
            Assert.Multiple(() =>
            {
                Assert.That(_myInfoPage.FirstNameTextBox.GetAttribute("value"), Is.EqualTo(firstName));
                Assert.That(_myInfoPage.MiddleNameTextBox.GetAttribute("value"), Is.EqualTo(middleName));
                Assert.That(_myInfoPage.LastNameTextBox.GetAttribute("value"), Is.EqualTo(lastName));
            });
            _myInfoPage.LastNameTextBox.SendKeys(Keys.Enter);
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
            _globalLocators.DashboardLink.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);
            Assert.That(_globalLocators.UserDropdown.Text, Is.EqualTo(expectedName));
        }

        [Test]
        public void CanUploadFile()
        {
            var filename = "cake_1.jpg";
            Dictionary<string, string> attachmentData;
            IWebElement attachmentCard;

            _myInfoPageHelpers.NavigateTo();
            _myInfoPageHelpers.UploadFile(filename);
            
            (attachmentCard, attachmentData) = _myInfoPageHelpers.GetAttachedFileData(filename);
            Assert.Multiple(() =>
            {
                Assert.That(attachmentData, Is.Not.Null, "No attachment data was parsed.");
                Assert.That(attachmentCard, Is.Not.Null);
                Assert.That(attachmentData["addedBy"], Is.EqualTo("Admin"));
                Assert.That(attachmentData["type"], Is.EqualTo("image/jpeg"));
                Assert.That(attachmentData["dateAdded"], Is.EqualTo(DateTime.Now.ToString("yyyy-dd-MM")));
            });
            _globalHelpers.DeleteRecord(attachmentCard);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
