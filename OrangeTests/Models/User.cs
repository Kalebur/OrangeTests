namespace OrangeHRMTests.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }
        public Employee Employee { get; set; }
        public bool IsEnabled { get; set; }
    }
}
