using OpenQA.Selenium;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Helpers
{
    public class MyInfoPageHelpers(IWebDriver driver, MyInfoPage myInfoPage, GlobalHelpers globalHelpers, GlobalLocators globalLocators)
    {
        private readonly IWebDriver _driver = driver;
        private readonly MyInfoPage _myInfoPage = myInfoPage;
        private readonly GlobalHelpers _globalHelpers = globalHelpers;
        private readonly GlobalLocators _globalLocators = globalLocators;

        public (IWebElement attachment, Dictionary<string, string>) GetAttachedFileData(string fileName)
        {
            Dictionary<string, string> attachmentData = null;
            IWebElement attachmentCard = null;

            foreach (var attachment in _myInfoPage.Attachments)
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
            _globalHelpers.Wait.Until(d => _myInfoPage.PersonalDetailsTabButton.Displayed);
        }

        public void UploadFile(string filename)
        {
            _globalHelpers.ClickViaActions(_myInfoPage.AddAttachmentButton);
            _myInfoPage.FileInput.SendKeys(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", filename));
            _globalHelpers.ClickViaActions(_myInfoPage.SaveAttachmentButton);
        }

        private Dictionary<string, string> GetAttachmentDataFromRow(IWebElement tableRow)
        {
            var attachmentData = new Dictionary<string, string>();
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
                { "fileName", _myInfoPage.ResponsiveAttachmentFilename(tableRow).Text.Trim() }
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
    }
}
