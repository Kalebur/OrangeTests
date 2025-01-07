using OpenQA.Selenium;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Helpers;

namespace OrangeHRMTests.Locators
{
    public class MyInfoPage(IWebDriver driver, GlobalHelpers globalHelpers, GlobalLocators globalLocators)
    {
        private readonly IWebDriver _driver = driver;
        private readonly GlobalHelpers _globalHelpers = globalHelpers;
        private readonly GlobalLocators _globalLocators = globalLocators;

        public IWebElement PersonalDetailsTabButton => _driver.FindElement(By.XPath("//div[@role='tab']//a[contains(text(), 'Personal Details')]"));
        public IWebElement FirstNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-firstname')]"));
        public IWebElement MiddleNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-middlename')]"));
        public IWebElement LastNameTextBox => _driver.FindElement(By.XPath("//input[contains(@class, 'orangehrm-lastname')]"));
        public IWebElement SavePersonalDetailsButton => _driver.FindElement(By.XPath("//h6[text()='Personal Details']//following-sibling::form//button[@type='submit']"));
        public IWebElement RecordCountSpan => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-attachment')]//hr[contains(@class, 'oxd-divider orangehrm-horizontal-margin')]//following-sibling::div//span"));
        public IWebElement FileInput => _driver.FindElement(By.XPath("//input[@type='file']"));
        public IWebElement ErrorMessageSpan => _driver.FindElement(By.XPath("//span[contains(@class, 'error-message')]"));
        public IWebElement FilenameDiv => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-file-input-div')]"));
        public IWebElement AddAttachmentButton => _driver.FindElement(By.XPath("//button//i[contains(@class, 'bi-plus')]//parent::button"));
        public IList<IWebElement> Attachments => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-table-body')]/child::div[contains(@class, 'oxd-table-card')]"));
        public IWebElement SaveAttachmentButton => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-attachment')]//button[@type='submit']"));
        public IWebElement ResponsiveAttachmentFilename(IWebElement element) => element.FindElement(By.XPath(".//div[contains(@class, 'card-item card-header-slot-content --left')]//div[contains(@class, 'data')]"));

        public (IWebElement attachment, Dictionary<string, string>) GetAttachedFileData(string fileName)
        {
            Dictionary<string, string> attachmentData = null;
            IWebElement attachmentCard = null;

            foreach (var attachment in Attachments)
            {
                _globalHelpers.ScrollTo(attachment);
                var data = GetAttachmentDataFromRow(attachment);
                if (data["fileName"] == fileName)
                {
                    attachmentData = data;
                    attachmentCard = attachment;
                    break;
                }
            }

            return (attachmentCard, attachmentData);
        }

        public void NavigateToPage()
        {
            _globalHelpers.LoginAs("admin", true);
            _globalHelpers.Wait.Until(d => _globalLocators.MyInfoLink.Displayed);
            _globalHelpers.ClickViaActions(_globalLocators.MyInfoLink);
            _globalHelpers.Wait.Until(d => PersonalDetailsTabButton.Displayed);
        }

        public void UploadFile(string filename)
        {
            _globalHelpers.ClickViaActions(AddAttachmentButton);
            FileInput.SendKeys(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", filename));
            _globalHelpers.ClickViaActions(SaveAttachmentButton);
        }

        private Dictionary<string, string> GetAttachmentDataFromRow(IWebElement tableRow)
        {
            Dictionary<string, string> attachmentData;
            if (_globalHelpers.GetWindowWidth() < 1000)
            {
                attachmentData = GetResponsiveAttachmentData(tableRow);
            }
            else
            {
                attachmentData = GetAttachmentData(tableRow);
            }

            return attachmentData;
        }

        private static Dictionary<string, string> GetAttachmentData(IWebElement tableRow)
        {
            var attachmentData = new Dictionary<string, string>();
            var dataFields = tableRow.FindElements(By.XPath(".//div[@role='cell']")).Skip(1).Take(6).ToList();
            attachmentData.Add("fileName", dataFields[0].Text);
            attachmentData.Add("description", dataFields[1].Text);
            attachmentData.Add("size", dataFields[2].Text);
            attachmentData.Add("type", dataFields[3].Text);
            attachmentData.Add("dateAdded", dataFields[4].Text);
            attachmentData.Add("addedBy", dataFields[5].Text);

            return attachmentData;
        }

        private Dictionary<string, string> GetResponsiveAttachmentData(IWebElement tableRow)
        {
            var attachmentData = new Dictionary<string, string>
            {
                { "fileName", ResponsiveAttachmentFilename(tableRow).Text.Trim() }
            };
            var dataFields = tableRow.FindElements(By.XPath(".//div[contains(@class, 'card-body-slot')]//div[@role='cell']//div[contains(@class, 'data')]"));
            if (dataFields.Count == 5)
            {
                attachmentData.Add("description", dataFields[0].Text);
                attachmentData.Add("size", dataFields[1].Text);
                attachmentData.Add("type", dataFields[2].Text);
                attachmentData.Add("dateAdded", dataFields[3].Text);
                attachmentData.Add("addedBy", dataFields[4].Text);
            }
            else
            {
                attachmentData.Add("size", dataFields[0].Text);
                attachmentData.Add("type", dataFields[1].Text);
                attachmentData.Add("dateAdded", dataFields[2].Text);
                attachmentData.Add("addedBy", dataFields[3].Text);
            }

            return attachmentData;
        }

        public void ClearNameFields()
        {
            FirstNameTextBox.ClearViaSendKeys();
            MiddleNameTextBox.ClearViaSendKeys();
            LastNameTextBox.ClearViaSendKeys();
        }

        public void FillNameFields(string firstName, string middleName = "", string lastName = "")
        {
            FirstNameTextBox.SendKeys(firstName);
            MiddleNameTextBox.SendKeys(middleName);
            LastNameTextBox.SendKeys(lastName);

        }

    }
}
