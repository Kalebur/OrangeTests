using OpenQA.Selenium;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Extensions;

namespace OrangeHRMTests.Locators
{
    public class LeavePage
    {
        private readonly IWebDriver _driver;
        private readonly GlobalHelpers _globalHelpers;
        private readonly GlobalLocators _globalLocators;
        private readonly Dictionary<int, string> monthsAsStrings = new()
            {
                { 1, "January" },
                { 2, "February" },
                { 3, "March" },
                { 4, "April" },
                { 5, "May" },
                { 6, "June" },
                { 7, "July" },
                { 8, "August" },
                { 9, "September" },
                { 10, "October" },
                { 11, "November" },
                { 12, "December" },
            };

        public LeavePage(IWebDriver driver, GlobalHelpers globalHelpers, GlobalLocators globalLocators)
        {
            _driver = driver;
            _globalHelpers = globalHelpers;
            _globalLocators = globalLocators;

        }

        public string InsufficientBalanceErrorText = "Balance not sufficient";

        public IWebElement ApplyLink => _driver.FindElement(By.XPath("//nav//a[contains(text(), 'Apply')]"));
        public IWebElement MyLeaveLink => _driver.FindElement(By.XPath("//nav//a[contains(text(), 'My Leave')]"));
        public IWebElement LeaveListLink => _driver.FindElement(By.XPath("//nav//a[contains(text(), 'Leave List')]"));
        public IWebElement ApplyButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement LeaveBalanceField => _driver.FindElement(By.XPath("//p[contains(@class, 'orangehrm-leave-balance-text')]"));
        public IWebElement LeaveTypeSelectElement => _driver.FindElement(By.XPath("//label[contains(text(), 'Leave Type')]//parent::div//following-sibling::div"));
        public IList<IWebElement> LeaveTypeOptions => _driver.FindElements(By.XPath("//label[contains(text(), 'Leave Type')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement FromDateInputField => _driver.FindElement(By.XPath("//label[contains(text(), 'From Date')]//parent::div//following-sibling::div//input"));
        public IWebElement ToDateInputField => _driver.FindElement(By.XPath("//label[contains(text(), 'To Date')]//parent::div//following-sibling::div//input"));
        public IWebElement CalendarDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-date-input-calendar')]"));
        public IWebElement MonthSelector => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-calendar-selector-month')]"));
        public IWebElement MonthsWrapper => MonthSelector.FindElement(By.XPath(".//following-sibling::ul"));
        public IWebElement YearSelector => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-calendar-selector-year')]"));
        public IWebElement YearWrapper => YearSelector.FindElement(By.XPath(".//following-sibling::ul"));
        public IList<IWebElement> Months => _driver.FindElements(By.XPath("//li[contains(@class, 'oxd-calendar-dropdown')]")).ToList();
        public IList<IWebElement> Years => _driver.FindElements(By.XPath("//li[contains(@class, 'oxd-calendar-dropdown')]")).ToList();
        public IList<IWebElement> Dates => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-calendar-dates-grid')]//div[contains(@class, 'oxd-calendar-date') and not(contains(@class, '-wrapper'))]")).ToList();
        public IWebElement PartialDaysSelectElement => _driver.FindElement(By.XPath("//label[contains(text(), 'Partial Days')]//parent::div//following-sibling::div"));
        public IList<IWebElement> PartialDaysOptions => _driver.FindElements(By.XPath("//label[contains(text(), 'Partial Days')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement DurationSelectElement => _driver.FindElement(By.XPath("//label[contains(text(), 'Duration')]//parent::div//following-sibling::div"));
        public IList<IWebElement> DurationOptions => _driver.FindElements(By.XPath("//label[contains(text(), 'Duration')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement CommentsTextArea => _driver.FindElement(By.XPath("//textarea"));
        public IWebElement RecordCountSpan => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-header-container')]//span[contains(@class, 'oxd-text--span')]"));
        public IList<IWebElement> LeaveRecords => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-table-body')]/child::div[contains(@class, 'oxd-table-card')]"));
        public IWebElement CancelLeaveButton => _driver.FindElement(By.XPath("//button[contains(@class, 'oxd-button--label-warn')]"));
        public IWebElement DropdownListBox => _driver.FindElement(By.XPath("//div[@role='listbox']"));
        public List<IWebElement> DropdownOptions => DropdownListBox.FindElements(By.XPath(".//div[@role='option']")).Skip(1).ToList();
        public List<IWebElement> RemoveStatusButtons => _driver.FindElements(By.XPath("//form//i[contains(@class, 'bi-x')]")).ToList();
        public IWebElement EmployeeNameInputField => _driver.FindElement(By.XPath("//label[contains(., 'Employee Name')]//parent::div//following-sibling::div//input"));

        

        public void SelectRandomLeaveType()
        {
            _globalHelpers.Wait.Until(d => LeaveTypeSelectElement.Displayed);
            LeaveTypeSelectElement.Click();
            _globalHelpers.SelectRandomElement(DropdownOptions);
        }

        public (DateTime startDate, DateTime endDate) GetRandomLeaveDates(int duration)
        {
            var currentDate = DateTime.Now.Date;
            var startDate = currentDate.AddDays(new Random().Next(10, 31));
            startDate = EnsureWorkingDay(startDate);
            var endDate = startDate.AddDays(duration);
            endDate = EnsureWorkingDay(endDate);

            return (startDate, endDate);
        }

        private DateTime EnsureWorkingDay(DateTime initialDate)
        {
            var adjustedDate = initialDate.Date;
            if (adjustedDate.DayOfWeek == DayOfWeek.Saturday) adjustedDate = adjustedDate.AddDays(2);
            else if (adjustedDate.DayOfWeek == DayOfWeek.Sunday) adjustedDate = adjustedDate.AddDays(1);

            return adjustedDate;
        }

        public void SelectDate(DateTime date)
        {
            _globalHelpers.Wait.Until(d => MonthSelector.Displayed);
            MonthSelector.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => MonthsWrapper.Displayed);
            Months.SelectItemByText(monthsAsStrings[date.Month]);
            _globalHelpers.ClickViaActions(YearSelector);
            _globalHelpers.Wait.Until(d => YearWrapper.Displayed);
            Years.SelectItemByText(date.Year.ToString());
            Dates.SelectItemByText(date.Day.ToString());
        }

        public List<IWebElement> FindRecordsForDateRangeAndStatus(DateTime startDate, DateTime endDate, string leaveStatus, bool matchEmployeeName = false)
        {
            var matchingRecords = new List<IWebElement>();
            foreach (var record in LeaveRecords)
            {
                var recordData = _globalHelpers.GetRowCells(record);
                (var fromDate, var toDate) = ParseRecordDates(recordData[1]);
                if (!(fromDate == startDate && toDate == endDate)) continue;
                if (!recordData[6].Text.Contains(leaveStatus)) continue;
                if (matchEmployeeName)
                {
                    var firstAndLastName = _globalLocators.UserName.Text.Split(' ');
                    if (!recordData[2].Text.Contains(firstAndLastName[0]) &&
                        !recordData[2].Text.Contains(firstAndLastName[1])) continue;
                }

                matchingRecords.Add(record);
            }

            return matchingRecords;
        }

        private (DateTime fromDate, DateTime? toDate) ParseRecordDates(IWebElement datesElement)
        {
            var dates = datesElement.Text.Split(" to ");
            var fromDateStrings = dates[0].Split('-');
            var fromDateAsString = $"{fromDateStrings[2]}/{fromDateStrings[1]}/{fromDateStrings[0]}";
            var fromDate = DateTime.Parse(fromDateAsString);
            DateTime? toDate = null;

            if (dates.Length > 1)
            {
                var toDateStrings = dates[1].Split('-');
                var toDateAsString = $"{toDateStrings[2]}/{toDateStrings[1]}/{toDateStrings[0]}";
                toDate = DateTime.Parse(toDateAsString);
            }

            return (fromDate, toDate);
        }

        public void ApplyForLeave(DateTime startDate, DateTime endDate)
        {
            NavigateToLeavePage();
            SelectRandomLeaveType();

            // Select Start Date
            FromDateInputField.SendKeys(startDate.ToString(_globalHelpers.dateFormatString));

            // Select End Date
            ToDateInputField.ClearViaSendKeys();
            ToDateInputField.SendKeys(endDate.ToString(_globalHelpers.dateFormatString));
            ToDateInputField.SendKeys(Keys.Tab);

            _globalHelpers.Wait.Until(d => PartialDaysSelectElement.Displayed);
            PartialDaysSelectElement.Click();
            PartialDaysOptions.SelectItemByText("All Days");
            _globalHelpers.Wait.Until(d => DurationSelectElement.Displayed);
            DurationSelectElement.Click();
            DurationOptions.SelectItemByText("Half Day - Morning");
            CommentsTextArea.SendKeys("Personal leave/vacation");
            ApplyButton.Submit();
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
        }

        private void NavigateToLeavePage()
        {
            _globalHelpers.LoginAs("admin", true);
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);
            _globalLocators.LeaveLink.Click();
            _globalHelpers.Wait.Until(d => ApplyLink.Displayed);
            ApplyLink.Click();
            _globalHelpers.Wait.Until(d => ApplyButton.Displayed);
        }

        public void AssertRecordExistsWithStatus(bool recordExists, IWebElement leaveRecord, string leaveStatus, string expectedStatus)
        {
            Assert.Multiple(() =>
            {
                Assert.That(recordExists, Is.True);
                Assert.That(leaveStatus, Is.EqualTo(expectedStatus));
            });
        }

        public void GotoLeaveList()
        {
            LeaveListLink.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.RecordsTable.Displayed);
        }

        public void CancelLeave()
        {
            CancelLeaveButton.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
            _globalHelpers.Wait.Until(d => LeaveRecords.Count > 0);
        }
    }
}
