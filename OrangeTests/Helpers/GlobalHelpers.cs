using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V85.IndexedDB;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OrangeHRMTests.Locators;
using OrangeHRMTests.Models;
using System;

namespace OrangeHRMTests.Helpers
{
    public class GlobalHelpers
    {
        private readonly IWebDriver _driver;
        private readonly Actions _actions;
        private readonly Random _random;
        private readonly GlobalLocators _globalLocators;
        private readonly LoginHelpers _loginHelpers;
        public readonly string dateFormatString;

        public GlobalHelpers(IWebDriver driver, Random random, GlobalLocators globalLocators)
        {
            _driver = driver;
            _actions = new(driver);
            _random = random;
            _globalLocators = globalLocators;
            _loginHelpers = new(_driver, new LoginPage(_driver), this, _globalLocators);
            dateFormatString = "yyyy-dd-MM";
        }

        public WebDriverWait Wait => new(_driver, TimeSpan.FromSeconds(10));

        public bool IsElementPresentOnPage(IWebElement element)
        {
            try
            {
                if (element.Displayed) return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return false;
        }

        public void ClickViaActions(IWebElement element)
        {
            _actions.ScrollToElement(element).Click(element).Perform();
        }

        public void ScrollTo(IWebElement element)
        {
            _actions.ScrollToElement(element).Perform();
        }

        public Int64 GetWindowWidth()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            var width = (Int64)js.ExecuteScript("return document.documentElement.clientWidth;");
            return width;
        }

        public User GenerateRandomUser()
        {
            var user = new User
            {
                Username = GetRandomUsername(),
                Password = GetRandomPassword(),
                UserRole = GetRandomUserRole(),
                Employee = new Employee()
            };
            AssignRandomName(user);
            user.IsEnabled = true;

            return user;
        }

        public void SelectRandomElement(IList<IWebElement> elements)
        {
            elements[_random.Next(elements.Count)].Click();
        }

        private UserRole GetRandomUserRole()
        {
            var allRoles = Enum.GetValues(typeof(UserRole));
            return (UserRole)allRoles.GetValue(_random.Next(allRoles.Length))!;
        }

        private string GetRandomUsername()
        {
            var usernames = new List<string>()
            {
                "DingleChingle",
                "mary.manchino",
                "fbaggins",
                "DarthJarJar",
                "rick.deckard",
            };

            var username = usernames[_random.Next(usernames.Count)];
            for (int i = 0; i < _random.Next(1, 5); i++)
            {
                username += _random.Next(1, 1000).ToString();
            }
            return username;
        }

        private void AssignRandomName(User user)
        {
            List<string> names =
            [
                "Jane",
                "Susan",
                "Essence",
                "Latoya",
                "Jason",
                "Alejandro",
                "Armand",
                "Raul",
                "Hiro",
                "Freddy",
                "Marie",
            ];

            List<string> surnames =
            [
                "Woods",
                "Nakamura",
                "Blackwell",
                "Dyson",
                "McDougal",
                "Zhang",
                "Yen",
                "Sun",
            ];

            user.Employee.FirstName = names[_random.Next(names.Count)];
            user.Employee.LastName = surnames[_random.Next(surnames.Count)];

            int middleNameCount = _random.Next(0, 4);
            if (middleNameCount > 0)
            {
                var middleNames = new string[middleNameCount];

                for (int i = 0; i < middleNameCount; i++)
                {
                    middleNames[i] = names[_random.Next(names.Count)];

                }

                user.Employee.MiddleName = string.Join(' ', middleNames);
            }
        }

        private string GetRandomPassword()
        {
            var passwords = new List<string>()
            {
                "FBcFICmH5",
                "6lvnXozQ",
                "5zp1tzjcBh",
                "8bY2tiHFA",
                "gZ7h7kuSVCHe",
                "u9mNOiVlQ",
                "xDPyIpT4FAL",
                "rymu9gaqoY",
                "eO5FtEztG",
                "WhuBog5rY",
            };

            return passwords[_random.Next(passwords.Count)];
        }

        public IList<IWebElement> GetRowCells(IWebElement tableRow)
        {
            return tableRow.FindElements(By.XPath(".//div[@role='cell']")).ToList();
        }

        public void DeleteRecord(IWebElement record)
        {
            record.FindElement(By.XPath(".//i[contains(@class, 'bi-trash')]//parent::button")).Click();
            Wait.Until(d => _globalLocators.ModalDeleteButton.Displayed);
            _globalLocators.ModalDeleteButton.Click();
        }

        public (string firstName, string middleName, string lastName) ParseEmployeeName(string name)
        {
            var splitName = name.Split(' ');
            var firstName = splitName.First();
            var lastName = splitName.Last();
            var middleName = string.Join(' ', splitName.Skip(1).Take(splitName.Length - 2));

            return (firstName, middleName, lastName);
        }

        public void LoginAs(string username, bool maximizeWindow = false)
        {
            _loginHelpers.LoginAs(username, maximizeWindow);
        }

        public void LoginWithCredentials(string username, string password, bool maximizeWindow = false)
        {
            _loginHelpers.LoginWithCredentials(username, password, maximizeWindow);
        }

        public void Logout()
        {
            _loginHelpers.Logout();
        }

        public string GetDisplayedUserName()
        {
            return _globalLocators.UserName.Text;
        }
    }

}
