﻿using OpenQA.Selenium;
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

        public AdminHelpers(IWebDriver driver, AdminPage adminPage, GlobalHelpers globalHelpers, Random random)
        {
            _driver = driver;
            _adminPage = adminPage;
            _globalHelpers = globalHelpers;
            _random = random;
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
            }
            else
            {
                var fields = tableRow.FindElements(By.XPath("//div[@role='cell']")).Skip(1).Take(4).ToList();

                newUser.Username = fields[0].Text;
                newUser.UserRole = (UserRole)Enum.Parse(typeof(UserRole), fields[1].Text);
                SetEmployeeNameFromField(newEmployee, fields[2]);
                newUser.IsEnabled = fields[3].Text.Trim() == "Enabled" ? true : false;
            }

            newUser.Employee = newEmployee;
            return newUser;
        }

        public string GetTestUsername()
        {
            var usernames = GetValidUsernames();
            if (usernames.Count == 1 && usernames[0] == "Admin")
            {
                throw new InvalidOperationException("Admin was the only user found. Cannot operate on Admin.");
            }

            string testUsername;
            do
            {
                testUsername = usernames[_random.Next(usernames.Count)];
            } while (testUsername == "Admin");
            return testUsername;
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
                    usernames.Add(user.FindElements(By.XPath(".//div[@role='cell']")).Skip(1).Take(1).First().Text.Trim());
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
