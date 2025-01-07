using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Tests
{
    public class MyInfoPageTests
    {
        private IWebDriver _driver;
        private GlobalHelpers _globalHelpers;
        private GlobalLocators _globalLocators;
        private MyInfoPage _myInfoPage;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, new Random(), _globalLocators);
            _myInfoPage = new MyInfoPage(_driver, _globalHelpers, _globalLocators);
        }

        [TestCase("Trini", "", "Quan", "Trini Quan")]
        [TestCase("Selena", "Ann", "Shepherd", "Selena Shepherd")]
        [TestCase("Tommy", "", "Oliver", "Tommy Oliver")]
        public void CanEditOwnName(string firstName, string middleName, string lastName, string expectedName)
        {
            _myInfoPage.NavigateToPage();

            _myInfoPage.FirstNameTextBox.ClearViaSendKeys();
            _myInfoPage.FirstNameTextBox.SendKeys(firstName);
            _myInfoPage.MiddleNameTextBox.ClearViaSendKeys();
            _myInfoPage.MiddleNameTextBox.SendKeys(middleName);
            _myInfoPage.LastNameTextBox.ClearViaSendKeys();
            _myInfoPage.LastNameTextBox.SendKeys(lastName);
            _myInfoPage.LastNameTextBox.Submit();
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
            _driver.Navigate().Refresh();
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);

            Assert.Multiple(() =>
            {
                Assert.That(_myInfoPage.FirstNameTextBox.GetAttribute("value"), Is.EqualTo(firstName));
                Assert.That(_myInfoPage.MiddleNameTextBox.GetAttribute("value"), Is.EqualTo(middleName));
                Assert.That(_myInfoPage.LastNameTextBox.GetAttribute("value"), Is.EqualTo(lastName));
                Assert.That(_globalLocators.UserDropdown.Text, Is.EqualTo(expectedName));
            });
        }

        [TestCase("TPS_Report1313.txt", "text/plain")]
        [TestCase("cake_1.jpg", "image/jpeg")]
        public void CanUploadFile(string filename, string fileType)
        {
            Dictionary<string, string> attachmentData;
            IWebElement attachmentCard;

            _myInfoPage.NavigateToPage();
            _myInfoPage.UploadFile(filename);
            //_globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
            _globalHelpers.Wait.Until(d => _myInfoPage.RecordCountSpan.Displayed);

            (attachmentCard, attachmentData) = _myInfoPage.GetAttachedFileData(filename);
            Assert.Multiple(() =>
            {
                Assert.That(attachmentData, Is.Not.Null, "No attachment data was parsed.");
                Assert.That(attachmentCard, Is.Not.Null);
                Assert.That(attachmentData["addedBy"], Is.EqualTo("Admin"));
                Assert.That(attachmentData["type"], Is.EqualTo(fileType));
                Assert.That(attachmentData["dateAdded"], Is.EqualTo(DateTime.Now.ToString(_globalHelpers.dateFormatString)));
            });
            _globalHelpers.DeleteRecord(attachmentCard);
        }

        [Test]
        public void Negative_CannotUploadFileLargerThan1MB()
        {
            var filename = "cake_2.png";

            _myInfoPage.NavigateToPage();
            _myInfoPage.UploadFile(filename);

            Assert.Multiple(() =>
            {
                Assert.That(_myInfoPage.FilenameDiv.Displayed, Is.True);
                Assert.That(_myInfoPage.ErrorMessageSpan.Displayed, Is.True);
                Assert.That(_myInfoPage.ErrorMessageSpan.Text, Is.EqualTo("Attachment Size Exceeded"));
            });
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
