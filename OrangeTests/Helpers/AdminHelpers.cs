using OpenQA.Selenium;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Locators;
using OrangeHRMTests.Models;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace OrangeHRMTests.Helpers
{
    public class AdminHelpers
    {
        private readonly IWebDriver _driver;
        private readonly AdminPage _adminPage;
        private readonly GlobalHelpers _globalHelpers;
        private readonly Random _random;

        public AdminHelpers(IWebDriver driver, AdminPage adminPage, GlobalHelpers globalHelpers)
        {
            _driver = driver;
            _adminPage = adminPage;
            _globalHelpers = globalHelpers;
            _random = new Random();
        }

        public IWebElement GetDeleteUserButton(IWebElement userRow)
        {
            return userRow.FindElement(By.XPath("//i[contains(@class, 'bi-trash')]/parent::button"));
        }

        public IWebElement GetEditUserButton(IWebElement userRow)
        {
            return userRow.FindElement(By.XPath("//i[contains(@class, 'bi-pencil-fill')]/parent::button"));
        }

        public int GetRecordCount()
        {
            string pattern = @"\((\d+)\)";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(_adminPage.RecordCountSpan.Text);

            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }

            return 0;
        }

        public void SearchForUserByUsername(string username)
        {
            if (!_globalHelpers.IsElementPresentOnPage(_adminPage.UsernameTextBox))
            {
                _adminPage.SystemUsersDisplayToggleButton.Click();
            }

            _globalHelpers.Wait.Until(d => _adminPage.UsernameTextBox.Displayed);
            _adminPage.UsernameTextBox.SendKeys(username);
            _adminPage.SearchButton.Click();
            _globalHelpers.Wait.Until(d => _adminPage.RecordCountSpan.Displayed);
        }

        public void EditUser(User newUser)
        {
            GetEditUserButton(_adminPage.Users[0]).Click();
            _globalHelpers.Wait.Until(d => _adminPage.EditEmployeeNameTextBox.Displayed);

            SelectUserRole(newUser.UserRole.ToString());
            var nameField = GetEmployeeNameElement();
            SetEmployeeNameFromField(newUser.Employee, nameField);
            nameField.Click();
            SelectUserStatus(newUser.IsEnabled);
            _adminPage.UsernameTextBox.ClearViaSendKeys();
            _adminPage.UsernameTextBox.SendKeys(newUser.Username);
            _adminPage.ChangePasswordCheckBox.Click();
            _globalHelpers.Wait.Until(d => _adminPage.PasswordTextBox.Displayed);
            _adminPage.PasswordTextBox.SendKeys(newUser.Password);
            _adminPage.ConfirmPasswordTextBox.SendKeys(newUser.Password);

            _adminPage.SaveUserButton.Click();
            _globalHelpers.Wait.Until(d => _adminPage.SystemUsersDisplayToggleButton.Displayed);
        }

        public IWebElement GetEmployeeNameElement()
        {
            _adminPage.EditEmployeeNameTextBox.ClearViaSendKeys();
            _adminPage.EditEmployeeNameTextBox.SendKeys("a");
            _globalHelpers.Wait.Until(d => _adminPage.FirstAutoCompleteName.Displayed);
            return _adminPage.EmployeeNames.GetRandomItem(_random);
        }

        public void SelectUserStatus(bool isEnabled)
        {
            _adminPage.StatusDropdown.Click();
            _globalHelpers.Wait.Until(d => _adminPage.StatusDropdownOptions.First().Displayed);
            _adminPage.StatusDropdownOptions.SelectItemByText(isEnabled ? "Enabled" : "Disabled");
        }

        public void SelectUserRole(string roleText)
        {
            _adminPage.UserRoleDropdown.Click();
            _globalHelpers.Wait.Until(d => _adminPage.UserRoleDropdownOptions.First().Displayed);
            _adminPage.UserRoleDropdownOptions.SelectItemByText(roleText);
        }

        public User GenerateRandomUser()
        {
            var user = new User();
            user.Username = GetRandomUsername();
            user.Password = GetRandomPassword();
            user.UserRole = GetRandomUserRole();
            user.Employee = new Employee();
            user.IsEnabled = _random.Next(1, 101) > 50 ? true : false;

            return user;
        }

        private UserRole GetRandomUserRole()
        {
            var allRoles = Enum.GetValues(typeof(UserRole));
            return (UserRole)allRoles.GetValue(_random.Next(allRoles.Length))!;
        }

        public string GetRandomUsername()
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

        public string GetRandomPassword()
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

        public User ParseUserTableRow(IWebElement tableRow)
        {
            var newUser = new User();
            var newEmployee = new Employee();

            if (_globalHelpers.GetWindowWidth() < 1000)
            {
                newUser.Username = _adminPage.ResponsiveUsername.Text;
                newUser.UserRole = ParseUserRole(_adminPage.ResponsiveUserRole.Text);
                newUser.IsEnabled = _adminPage.ResponsiveUserStatus.Text == "Enabled" ? true : false;
                SetEmployeeNameFromField(newEmployee, _adminPage.ResponsiveEmployeeName);
                newUser.Employee = newEmployee;
            }
            else
            {
                var fields = tableRow.FindElements(By.XPath("//div[@role='cell']")).Skip(1).Take(4).ToList();

                newUser.Username = fields[0].Text;
                newUser.UserRole = (UserRole)Enum.Parse(typeof(UserRole), fields[1].Text);
                SetEmployeeNameFromField(newEmployee, fields[2]);
                newUser.IsEnabled = fields[3].Text.Trim() == "Enabled" ? true : false;
            }

            return newUser;
        }

        public string GetTestUsername()
        {
            var usernames = GetValidUsernames();
            return usernames[_random.Next(usernames.Count)];
        }

        private List<string> GetValidUsernames()
        {
            List<string> usernames = [];
            if (_globalHelpers.GetWindowWidth() < 1000)
            {
                var usernameFields = _driver.FindElements(By.XPath("//div[contains(text(), 'Username')]//following-sibling::div"));
                foreach (var field in usernameFields)
                {
                    usernames.Add(field.Text);
                }
            }
            else
            {
                foreach (var user in _adminPage.Users)
                {
                    usernames.Add(user.FindElements(By.XPath("//div[@role='cell']")).Skip(1).Take(1).First().Text);
                }
            }

            return usernames;
        }

        public void SetEmployeeNameFromField(Employee newEmployee, IWebElement nameField)
        {
            var nameParts = nameField.Text.Split(' ');
            newEmployee.FirstName = nameParts.First();
            newEmployee.LastName = nameParts.Last();
            newEmployee.MiddleName = string.Join(" ", nameParts.Skip(1).Take(nameParts.Count() - 2));
        }

        private UserRole ParseUserRole(string userRole)
        {
            return (UserRole)Enum.Parse(typeof(UserRole), userRole);
        }
    }
}
