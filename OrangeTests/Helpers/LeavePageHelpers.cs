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

        public (bool recordExists, string leaveStatus) GetLeaveRecordForDateRange(DateTime startDate, DateTime endDate)
        {
            return (true, "Pending");
        }
    }
}
