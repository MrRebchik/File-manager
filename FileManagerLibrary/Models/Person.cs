namespace FileManagerLibrary.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public Person(string email, string hashPassword)
        {
            Email = email;
            HashPassword = hashPassword;
        }
    }
}
