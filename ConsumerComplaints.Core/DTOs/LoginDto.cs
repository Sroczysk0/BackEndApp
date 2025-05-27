namespace ConsumerComplaints.Core.DTOs
{
    /// <summary>
    /// Model DTO służący do logowania użytkownika.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Login użytkownika.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Hasło użytkownika.
        /// </summary>
        public string Password { get; set; }
    }
}