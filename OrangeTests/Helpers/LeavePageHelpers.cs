using OpenQA.Selenium;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Locators;

namespace OrangeHRMTests.Helpers
{
    public class LeavePageHelpers(LeavePage leavePage, GlobalHelpers globalHelpers)
    {
        private readonly LeavePage _leavePage = leavePage;
        private readonly GlobalHelpers _globalHelpers = globalHelpers;
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
            _globalHelpers.SelectRandomElement(_leavePage.LeaveTypeOptions);
        }

        public void SelectDate(DateTime date)
        {
            _globalHelpers.Wait.Until(d => _leavePage.MonthSelector.Displayed);
            _leavePage.MonthSelector.ClickViaJavaScript();
            _globalHelpers.Wait.Until(d => _leavePage.MonthsWrapper.Displayed);
            //_globalHelpers.SelectElementByText(_leavePage.Months, monthsAsStrings[date.Month]);
            _leavePage.Months.SelectItemByText(monthsAsStrings[date.Month]);
            //_leavePage.YearSelector.ClickViaJavaScript();
            _globalHelpers.ClickViaActions(_leavePage.YearSelector);
            _globalHelpers.Wait.Until(d => _leavePage.YearWrapper.Displayed);
            //_globalHelpers.SelectElementByText(_leavePage.Years, date.Year.ToString());
            //_globalHelpers.SelectElementByText(_leavePage.Dates, date.Day.ToString());
            _leavePage.Years.SelectItemByText(date.Year.ToString());
            _leavePage.Dates.SelectItemByText(date.Day.ToString());
        }

        public (bool recordExists, string leaveStatus, IWebElement leaveRecord) GetLeaveRecordForDateRange(DateTime startDate, DateTime endDate)
        {
            foreach (var record in _leavePage.LeaveRecords)
            {
                var recordData = _globalHelpers.GetRowCells(record);
                (var fromDate, var toDate) = ParseRecordDates(recordData[1]);
                if (fromDate == startDate && toDate == endDate)
                {
                    return (true, recordData[6].Text.Split(' ')[0], record);
                }
            }

            return (false, "Not Found.", null);
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
    }
}
