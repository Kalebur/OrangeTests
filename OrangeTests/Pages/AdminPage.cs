using OpenQA.Selenium;
using OrangeHRMTests.Extensions;
using OrangeHRMTests.Helpers;
using OrangeHRMTests.Models;
using System.Text.RegularExpressions;

namespace OrangeHRMTests.Locators
{
    public class AdminPage(IWebDriver driver, GlobalHelpers globalHelpers, GlobalLocators globalLocators, Random random)
    {
        private readonly IWebDriver _driver = driver;
        private readonly GlobalHelpers _globalHelpers = globalHelpers;
        private readonly GlobalLocators _globalLocators = globalLocators;
        private readonly Random _random = random;

        public string Url => "https://opensource-demo.orangehrmlive.com/web/index.php/admin/viewSystemUsers";

        public IWebElement SystemUsersDisplayToggleButton => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-table-filter-header-options')]//button"));
        public IWebElement UsernameTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Username')]//parent::div//following-sibling::div//input"));
        public IWebElement UserRoleDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'User Role')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-text--active')]"));
        public IList<IWebElement> UserRoleDropdownOptions => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'User Role')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement EmployeeNameTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Employee Name')]//parent::div//following-sibling::div//input"));
        public IWebElement StatusDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Status')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-text--active')]"));
        public IList<IWebElement> StatusDropdownOptions => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Status')]//parent::div//following-sibling::div//div[contains(@class, 'oxd-select-dropdown')]//div")).Skip(1).ToList();
        public IWebElement SearchButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement AddUserButton => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-paper-container')]//button[contains(@class, 'oxd-button')]"));
        public IWebElement RecordCountSpan => _driver.FindElement(By.XPath("//div[contains(@class, 'orangehrm-horizontal-padding')]//span"));
        public IList<IWebElement> Users => _driver.FindElements(By.XPath("//div[contains(@class, 'oxd-table-body')]/child::div[contains(@class, 'oxd-table-card')]"));
        public IWebElement EditEmployeeNameTextBox => _driver.FindElement(By.XPath("//input[@placeholder='Type for hints...']"));
        public IWebElement ChangePasswordCheckBox => _driver.FindElement(By.XPath("//i[contains(@class, 'bi-check')]"));
        public IWebElement PasswordTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'user-password-cell')]//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Password')]//parent::div//following-sibling::div//input"));
        public IWebElement ConfirmPasswordTextBox => _driver.FindElement(By.XPath("//div[contains(@class, 'user-password-cell')]/following-sibling::div//div[contains(@class, 'oxd-input-group__label-wrapper')]//label[contains(text(), 'Password')]//parent::div//following-sibling::div//input"));
        public IWebElement CancelButton => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-form-actions')]//button[@type='button']"));
        public IWebElement SaveUserButton => _driver.FindElement(By.XPath("//button[@type='submit']"));
        public IWebElement EmployeeAutoCompleteDropdown => _driver.FindElement(By.XPath("//div[contains(@class, 'oxd-autocomplete-dropdown')]"));
        public IWebElement ResponsiveEmployeeName => _driver.FindElement(By.XPath("//div[contains(text(), 'Employee Name')]//following-sibling::div"));
        public IList<IWebElement> ResponsiveUserFields => _driver.FindElements(By.XPath("//div[contains(@class, 'card-body-slot')]//div[@role='cell']"));
        public IWebElement ResponsiveUsername => _driver.FindElement(By.XPath("//div[contains(text(), 'Username')]//following-sibling::div"));
        public IWebElement ResponsiveUserRole => _driver.FindElement(By.XPath("//div[contains(text(), 'User Role')]//following-sibling::div"));
        public IWebElement ResponsiveUserStatus => _driver.FindElement(By.XPath("//div[contains(text(), 'Status')]//following-sibling::div"));
        public IList<IWebElement> EmployeeNames => EmployeeAutoCompleteDropdown.FindElements(By.XPath("//div[contains(@class, 'autocomplete-option')]/span"));
        public IWebElement FirstAutoCompleteName => _driver.FindElement(By.XPath("//div[contains(@class, 'autocomplete-option')]/span"));
        public IWebElement ConfigurationDropdownButton => _driver.FindElements(By.XPath("//span[@class='oxd-topbar-body-nav-tab-item']")).Last();
        public IWebElement ConfigurationDropdownList => _driver.FindElement(By.XPath("//span[@class='oxd-topbar-body-nav-tab-item']//following-sibling::ul"));
        public IWebElement ConfigurationButton => HeaderButtonList.Last();
        public IWebElement ConfigurationDropdown => ConfigurationButton.FindElement(By.TagName("ul"));
        public IList<IWebElement> ConfigurationOptions => ConfigurationDropdown.FindElements(By.TagName("li"));
        public IList<IWebElement> LocalizationLabels => _driver.FindElements(By.TagName("label"));
        public IWebElement LanguageDropdownButton => LocalizationLabels[0].FindElement(By.XPath(".//parent::div//following-sibling::div"));
        public IWebElement ListBox => _driver.FindElement(By.XPath("//div[@role='listbox']"));
        public IList<IWebElement> ListBoxOptions => ListBox.FindElements(By.XPath(".//div[@role='option']"));
        public IList<IWebElement> HeaderButtonList => _driver.FindElements(By.XPath("//header//nav//ul//li"));
        public IWebElement Menu => _driver.FindElement(By.XPath("//ul[@role='menu']"));
        public IList<IWebElement> MenuOptions => Menu.FindElements(By.TagName("li"));

        public IWebElement GetEditUserButton(IWebElement userRow)
        {
            return userRow.FindElement(By.XPath(".//i[contains(@class, 'bi-pencil-fill')]/parent::button"));
        }

        public int GetRecordCount()
        {
            string pattern = @"\((\d+)\)";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(RecordCountSpan.Text);

            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            return 0;
        }

        public void SearchForUserByUsername(string username)
        {
            if (!_globalHelpers.IsElementPresentOnPage(UsernameTextBox))
            {
                SystemUsersDisplayToggleButton.Click();
            }

            _globalHelpers.Wait.Until(d => UsernameTextBox.Displayed);
            UsernameTextBox.SendKeys(username);
            SearchButton.Click();
            _globalHelpers.Wait.Until(d => RecordCountSpan.Displayed);
        }

        public void EditUser(User newUser)
        {
            GetEditUserButton(Users[0]).Click();
            _globalHelpers.Wait.Until(d => EditEmployeeNameTextBox.Displayed);

            SelectUserRole(newUser.UserRole.ToString());
            var nameField = GetEmployeeNameElement();
            SetEmployeeNameFromField(newUser.Employee, nameField);
            nameField.Click();
            SelectUserStatus(newUser.IsEnabled);
            CompleteUserFormAs(newUser);
        }

        private void CompleteUserFormAs(User newUser)
        {
            UsernameTextBox.ClearViaSendKeys();
            UsernameTextBox.SendKeys(newUser.Username);
            ChangePasswordCheckBox.Click();
            _globalHelpers.Wait.Until(d => PasswordTextBox.Displayed);
            PasswordTextBox.SendKeys(newUser.Password);
            ConfirmPasswordTextBox.SendKeys(newUser.Password);
            ConfirmPasswordTextBox.Submit();
            _globalHelpers.Wait.Until(d => SystemUsersDisplayToggleButton.Displayed);
        }

        public IWebElement GetEmployeeNameElement()
        {
            EditEmployeeNameTextBox.ClearViaSendKeys();
            EditEmployeeNameTextBox.SendKeys("a");
            _globalHelpers.Wait.Until(d => FirstAutoCompleteName.Displayed);
            return EmployeeNames.GetRandomItem(_random);
        }

        public void SelectUserStatus(bool isEnabled)
        {
            StatusDropdown.Click();
            _globalHelpers.Wait.Until(d => StatusDropdownOptions.First().Displayed);
            StatusDropdownOptions.SelectItemByText(isEnabled ? "Enabled" : "Disabled");
        }

        public void SelectUserRole(string roleText)
        {
            UserRoleDropdown.Click();
            _globalHelpers.Wait.Until(d => UserRoleDropdownOptions.First().Displayed);
            UserRoleDropdownOptions.SelectItemByText(roleText);
        }

        public User ParseUserTableRow(IWebElement tableRow)
        {
            var newUser = new User();
            var newEmployee = new Employee();

            if (_globalHelpers.GetWindowWidth() < 1000)
            {
                newUser.Username = ResponsiveUsername.Text;
                newUser.UserRole = ParseUserRole(ResponsiveUserRole.Text);
                newUser.IsEnabled = ResponsiveUserStatus.Text == "Enabled" ? true : false;
                SetEmployeeNameFromField(newEmployee, ResponsiveEmployeeName);
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
                SeedUsers();
            }

            usernames = GetValidUsernames();
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
                foreach (var user in Users)
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

        public void SeedUsers()
        {
            var numUsersToAdd = _random.Next(3, 6);
            for (int i = 0; i < numUsersToAdd; i++)
            {
                var newUser = _globalHelpers.GenerateRandomUser();
                _globalHelpers.Wait.Until(d => AddUserButton.Displayed);
                AddUserButton.ClickViaJavaScript();
                _globalHelpers.Wait.Until(d => ConfirmPasswordTextBox.Displayed);
                AddUser(newUser);
                _globalHelpers.Wait.Until(d => RecordCountSpan.Displayed);
            }
        }

        public void AddUser(User user)
        {
            SelectUserRole(user.UserRole.ToString());
            var name = GetEmployeeNameElement();
            SetEmployeeNameFromField(user.Employee, name);
            name.Click();
            SelectUserStatus(user.IsEnabled);
            UsernameTextBox.SendKeys(user.Username);
            PasswordTextBox.SendKeys(user.Password);
            ConfirmPasswordTextBox.SendKeys(user.Password);
            ConfirmPasswordTextBox.Submit();
        }

        public void NavigateToAdminPage()
        {
            _globalHelpers.LoginAs("admin", true);
            _globalHelpers.Wait.Until(d => _globalLocators.AdminLink.Displayed);
            _globalLocators.AdminLink.Click();
            _globalHelpers.Wait.Until(d => RecordCountSpan.Displayed);
        }

        public void AssertUpdateWasSuccessful(User testUserData, User updatedUserData)
        {
            Assert.Multiple(() =>
            {
                Assert.That(updatedUserData.Username, Is.EqualTo(testUserData.Username));
                Assert.That(updatedUserData.Employee.FirstName, Is.EqualTo(testUserData.Employee.FirstName));
                Assert.That(updatedUserData.Employee.LastName, Is.EqualTo(testUserData.Employee.LastName));
                Assert.That(updatedUserData.UserRole, Is.EqualTo(testUserData.UserRole));
                Assert.That(updatedUserData.IsEnabled, Is.EqualTo(testUserData.IsEnabled));
            });
        }

        public void SetSiteLanguage()
        {
            NavigateToAdminPage();
            NavigateToLocalizationPage();
            SelectEnglishLanguage();
            ClickSubmitButton();
            Task.Delay(5000).Wait();
            _globalHelpers.Logout();
        }

        public void NavigateToLocalizationPage()
        {
            ConfigurationButton.Click();
            _globalHelpers.Wait.Until(d => Menu.Displayed && MenuOptions.Count >= 3);
            MenuOptions[2].Click();
        }

        private void SelectEnglishLanguage()
        {
            _globalHelpers.Wait.Until(d => LocalizationLabels.Count > 0);
            LanguageDropdownButton.Click();
            _globalHelpers.Wait.Until(d => ListBoxOptions.Count >= 4);
            ListBoxOptions[3].Click();
        }

        private void ClickSubmitButton()
        {
            _globalLocators.SubmitButton.Click();
        }
    }
}
