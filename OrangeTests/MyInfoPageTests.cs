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
        private LoginHelpers _loginHelpers;
        private MyInfoPage _myInfoPage;
        private MyInfoPageHelpers _myInfoPageHelpers;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _globalLocators = new GlobalLocators(_driver);
            _globalHelpers = new GlobalHelpers(_driver, new Random(), _globalLocators);
            _loginHelpers = new LoginHelpers(_driver, new LoginPage(_driver), _globalHelpers, _globalLocators);
            _myInfoPage = new MyInfoPage(_driver);
            _myInfoPageHelpers = new MyInfoPageHelpers(_driver, _myInfoPage, _globalHelpers);
        }

        //[TestCase("John", "William", "Waterhouse", "John Waterhouse")]
        //[TestCase("Jane", "Marie", "McDougal", "Jane McDougal")]
        //[TestCase("Trini", "", "Quan", "Trini Quan")]
        [TestCase("Maxwell", "Hunter", "Sloan", "Maxwell Sloan")]
        [TestCase("Selena", "Ann", "Shepherd", "Selena Shepherd")]
        public void CanEditOwnName(string firstName, string middleName, string lastName, string expectedName)
        {
            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.MyInfoLink.Displayed);
            _globalLocators.MyInfoLink.Click();
            _globalHelpers.Wait.Until(d => _myInfoPage.PersonalDetailsTabButton.Displayed);

            _myInfoPage.FirstNameTextBox.ClearViaSendKeys();
            _myInfoPage.FirstNameTextBox.SendKeys(firstName);
            Assert.That(_myInfoPage.FirstNameTextBox.GetAttribute("value"), Is.EqualTo(firstName));
            _myInfoPage.MiddleNameTextBox.ClearViaSendKeys();
            _myInfoPage.MiddleNameTextBox.SendKeys(middleName);
            Assert.That(_myInfoPage.MiddleNameTextBox.GetAttribute("value"), Is.EqualTo(middleName));
            _myInfoPage.LastNameTextBox.ClearViaSendKeys();
            _myInfoPage.LastNameTextBox.SendKeys(lastName);
            Assert.That(_myInfoPage.LastNameTextBox.GetAttribute("value"), Is.EqualTo(lastName));

            _myInfoPage.SavePersonalDetailsButton.Click();
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

            _loginHelpers.LoginAs("admin");
            _globalHelpers.Wait.Until(d => _globalLocators.MyInfoLink.Displayed);
            _globalHelpers.ClickViaActions(_globalLocators.MyInfoLink);
            _globalHelpers.Wait.Until(d => _myInfoPage.PersonalDetailsTabButton.Displayed);

            _globalHelpers.ClickViaActions(_myInfoPage.AddAttachmentButton);
            Assert.That(_myInfoPage.FilenameDiv.Displayed, Is.True);
            _myInfoPage.FileInput.SendKeys(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", filename));
            _globalHelpers.ClickViaActions(_myInfoPage.SaveAttachmentButton);
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
            _globalHelpers.Wait.Until(d => _myInfoPage.RecordCountSpan.Displayed);
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
