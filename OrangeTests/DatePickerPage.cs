using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OrangeHRMTests
{
    public class DatePickerPage
    {
        private readonly IWebDriver _driver;
        private readonly Dictionary<int, string> monthsAsStrings = new()
            {
                { 1, "Jan" },
                { 2, "Feb" },
                { 3, "Mar" },
                { 4, "Apr" },
                { 5, "May" },
                { 6, "Jun" },
                { 7, "Jul" },
                { 8, "Aug" },
                { 9, "Sep" },
                { 10, "Oct" },
                { 11, "Nov" },
                { 12, "Dec" },
            };

        public DatePickerPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string Url = "https://air-datepicker.com/";

        public IWebElement Logo => _driver.FindElement(By.XPath("//h2[@class='logo']"));
        public IWebElement CalendarTitleButton => _driver.FindElement(By.XPath("//div[contains(@class, 'air-datepicker-nav--title')]"));
        public IWebElement Calendar => _driver.FindElement(By.XPath("//div[contains(@class, 'air-datepicker-body--cells -days')]"));
        public IWebElement CalendarNextButton => _driver.FindElement(By.XPath("//div[@data-action='next']"));
        public IWebElement CalendarPreviousButton => _driver.FindElement(By.XPath("//div[@data-action='prev']"));
        public List<IWebElement> DateButtons => Calendar.FindElements(By.XPath("//div[contains(@class, 'air-datepicker-cell -day-') and not(contains(@class, '-other-month-'))]")).ToList();
        public List<IWebElement> MonthButtons => Calendar.FindElements(By.XPath("//div[contains(@class, 'air-datepicker-cell -month-')]")).ToList();
        public List<IWebElement> YearButtons => Calendar.FindElements(By.XPath("//div[contains(@class, 'air-datepicker-cell -year-') and not(contains(@class, '-other-decade-'))]")).ToList();

        public void SelectDate(string date)
        {
            var today = DateOnly.Parse(DateTime.Today.ToString().Split(' ')[0]);
            var targetDate = DateOnly.Parse(date);
            SelectYear(targetDate.Year, today.Year);
            SelectMonth(targetDate.Month);
            SelectDay(targetDate.Day);
        }

        public bool TargetDateIsSelected(string date)
        {
            var targetDate = DateOnly.Parse(date);
            var targetDay = targetDate.Day;
            var targetMonth = targetDate.Month;
            var targetYear = targetDate.Year;

            var selectedDay = DateButtons.Where(button => button.Selected).First().Text;
            var selectedMonth = CalendarTitleButton.Text.Split(',', StringSplitOptions.TrimEntries)[0];
            var selectedYear = int.Parse(CalendarTitleButton.Text.Split(',', StringSplitOptions.TrimEntries)[1]);

            return targetDay.ToString() == selectedDay
                && selectedMonth.Contains(monthsAsStrings[targetMonth])
                && selectedYear == targetYear;
        }

        private void SelectDay(int day)
        {
            DateButtons.Where(button => button.Text == day.ToString()).First().Click();
        }

        private void SelectMonth(int month)
        {

            MonthButtons.Where(button => button.Text == monthsAsStrings[month]).First().Click();
        }

        private void SelectYear(int targetYear, int currentYear)
        {
            if (targetYear != currentYear)
            {
                CalendarTitleButton.Click();
                CalendarTitleButton.Click();
                (int minYear, int maxYear) = GetYearRange();

                if (YearWithinRange(minYear, maxYear, targetYear))
                {
                    YearButtons.Where(button => button.Text == targetYear.ToString()).First().Click();
                }
                else
                {
                    while (!YearWithinRange(minYear, maxYear, targetYear))
                    {
                        if (targetYear > currentYear)
                        {
                            CalendarNextButton.Click();
                        }
                        else
                        {
                            CalendarPreviousButton.Click();
                        }

                        (minYear, maxYear) = GetYearRange();
                    }

                    YearButtons.Where(button => button.Text == targetYear.ToString()).First().Click();
                }
            }
        }

        private bool YearWithinRange(int minYear, int maxYear, int currentYear)
        {
            return currentYear >= minYear && currentYear <= maxYear;
        }

        private (int, int) GetYearRange()
        {
            var rangeValues = CalendarTitleButton.Text.Split(' ');
            var minYear = int.Parse(rangeValues[0]);
            var maxYear = int.Parse(rangeValues[2]);

            return (minYear, maxYear);
        }
    }
}
