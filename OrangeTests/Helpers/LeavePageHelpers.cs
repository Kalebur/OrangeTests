using OpenQA.Selenium;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Helpers
{
    public class LeavePageHelpers(LeavePage leavePage, GlobalHelpers globalHelpers, GlobalLocators globalLocators)
    {
        private readonly LeavePage _leavePage = leavePage;
        private readonly GlobalHelpers _globalHelpers = globalHelpers;
        private readonly GlobalLocators _globalLocators = globalLocators;
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

        public void SelectRandomLeaveType()
        {
            _globalHelpers.Wait.Until(d => _leavePage.LeaveTypeSelectElement.Displayed);
            _leavePage.LeaveTypeSelectElement.Click();
            _globalHelpers.SelectRandomElement(_leavePage.DropdownOptions);
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
            _globalHelpers.Wait.Until(d => _leavePage.MonthSelector.Displayed);
            _leavePage.MonthSelector.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => _leavePage.MonthsWrapper.Displayed);
            _leavePage.Months.SelectItemByText(monthsAsStrings[date.Month]);
            _globalHelpers.ClickViaActions(_leavePage.YearSelector);
            _globalHelpers.Wait.Until(d => _leavePage.YearWrapper.Displayed);
            _leavePage.Years.SelectItemByText(date.Year.ToString());
            _leavePage.Dates.SelectItemByText(date.Day.ToString());
        }

        public List<IWebElement> GetLeaveRecordForDateRange(DateTime startDate, DateTime endDate, string leaveStatus)
        {
            var matchingRecords = new List<IWebElement>();
            foreach (var record in _leavePage.LeaveRecords)
            {
                var recordData = _globalHelpers.GetRowCells(record);
                (var fromDate, var toDate) = ParseRecordDates(recordData[1]);
                if (fromDate == startDate && toDate == endDate)
                {
                    if (recordData[6].Text.Contains(leaveStatus)) matchingRecords.Add(record);
                }
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
            _leavePage.FromDateInputField.SendKeys(startDate.ToString(_globalHelpers.dateFormatString));

            // Select End Date
            _leavePage.ToDateInputField.ClearViaSendKeys();
            _leavePage.ToDateInputField.SendKeys(endDate.ToString(_globalHelpers.dateFormatString));
            _leavePage.ToDateInputField.SendKeys(Keys.Tab);

            _globalHelpers.Wait.Until(d => _leavePage.PartialDaysSelectElement.Displayed);
            _leavePage.PartialDaysSelectElement.Click();
            _leavePage.PartialDaysOptions.SelectItemByText("All Days");
            _globalHelpers.Wait.Until(d => _leavePage.DurationSelectElement.Displayed);
            _leavePage.DurationSelectElement.Click();
            _leavePage.DurationOptions.SelectItemByText("Half Day - Morning");
            _leavePage.CommentsTextArea.SendKeys("Personal leave/vacation");
            _leavePage.ApplyButton.Submit();
            _globalHelpers.Wait.Until(d => _globalLocators.SuccessAlert.Displayed);
        }

        private void NavigateToLeavePage()
        {
            _globalHelpers.LoginAs("admin", true);
            _globalHelpers.Wait.Until(d => _globalLocators.UserDropdown.Displayed);
            _globalLocators.LeaveLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyLink.Displayed);
            _leavePage.ApplyLink.Click();
            _globalHelpers.Wait.Until(d => _leavePage.ApplyButton.Displayed);
        }

        public void AssertRecordExistsWithStatus(bool recordExists, IWebElement leaveRecord, string leaveStatus, string expectedStatus)
        {
            Assert.Multiple(() =>
            {
                Assert.That(recordExists, Is.True);
                Assert.That(leaveStatus, Is.EqualTo(expectedStatus));
            });
        }

        public void FindRecordsByDateRangeAndStatus(DateTime startDate, DateTime endDate, string leaveStatus)
        {
            _leavePage.FromDateInputField.ClearViaSendKeys();
            _leavePage.FromDateInputField.SendKeys(startDate.ToString(_globalHelpers.dateFormatString));
            _leavePage.ToDateInputField.ClearViaSendKeys();
            _leavePage.ToDateInputField.SendKeys(endDate.ToString(_globalHelpers.dateFormatString));
            _leavePage.FromDateInputField.Submit();
            _globalHelpers.Wait.Until(d => _leavePage.LeaveRecords.Count > 0);
        }

        public void GotoLeaveList()
        {
            _leavePage.LeaveListLink.Click();
            _globalHelpers.Wait.Until(d => _globalLocators.RecordsTable.Displayed);
        }
    }
}
