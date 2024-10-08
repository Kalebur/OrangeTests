﻿using OpenQA.Selenium;
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

        public (IWebElement attachment, Dictionary<string, string>?) GetAttachedFileData(string fileName)
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

        private Dictionary<string, string> GetAttachmentDataFromRow(IWebElement tableRow)
        {
            var attachmentData = new Dictionary<string, string>();
            if (_globalHelpers.GetWindowWidth() < 1000)
            {
                attachmentData.Add("fileName", _myInfoPage.ResponsiveAttachmentFilename.Text);
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
            }
            else
            {
                var dataFields = tableRow.FindElements(By.XPath(".//div[@role='cell']")).Skip(1).Take(6).ToList();
                attachmentData.Add("fileName", dataFields[0].Text);
                attachmentData.Add("description", dataFields[1].Text);
                attachmentData.Add("size", dataFields[2].Text);
                attachmentData.Add("type", dataFields[3].Text);
                attachmentData.Add("dateAdded", dataFields[4].Text);
                attachmentData.Add("addedBy", dataFields[5].Text);
            }

            return attachmentData;
        }
    }
}
