using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OrangeHRMTests.Locators;
using OrangeHRMTests.Models;
using System;

namespace OrangeHRMTests.Helpers
{
    public class GlobalHelpers(IWebDriver driver, Random random)
    {
        private readonly IWebDriver _driver = driver;
        private readonly Actions _actions = new Actions(driver);
        private readonly Random _random = random;

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
            _actions.ScrollToElement(element);
            _actions.Click();
        }

        public Int64 GetWindowWidth()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            var width = (Int64)js.ExecuteScript("return document.documentElement.clientWidth;");
            return width;
        }

        public User GenerateRandomUser()
        {
            var user = new User();
            user.Username = GetRandomUsername();
            user.Password = GetRandomPassword();
            user.UserRole = GetRandomUserRole();
            user.Employee = new Employee();
            AssignRandomName(user);
            user.IsEnabled = _random.Next(1, 101) > 50 ? true : false;

            return user;
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
            List<string> names = new()
            {
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
            };

            List<string> surnames = new()
            {
                "Woods",
                "Nakamura",
                "Blackwell",
                "Dyson",
                "McDougal",
                "Zhang",
                "Yen",
                "Sun",
            };

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

    }

}
