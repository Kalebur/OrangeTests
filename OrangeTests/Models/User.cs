namespace OrangeHRMTests.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRole UserRole { get; set; }
        public string EmployeeName { get; set; }
        public bool IsEnabled { get; set; }

    }
}
