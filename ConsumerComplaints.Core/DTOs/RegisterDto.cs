using System;
using System.ComponentModel.DataAnnotations;

namespace ConsumerComplaints.Core.DTOs
{
    /// <summary>
    /// Model DTO do rejestracji nowego użytkownika.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Nazwa użytkownika (login) – min. 4 znaki.
        /// </summary>
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        [MinLength(4, ErrorMessage = "Nazwa użytkownika musi mieć co najmniej 4 znaki.")]
        public string UserName { get; set; }

        /// <summary>
        /// Adres e-mail użytkownika.
        /// </summary>
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string Email { get; set; }

        /// <summary>
        /// Hasło – minimum 6 znaków.
        /// </summary>
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków.")]
        public string Password { get; set; }

        /// <summary>
        /// Imię użytkownika.
        /// </summary>
        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Nazwisko użytkownika.
        /// </summary>
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string LastName { get; set; }

        /// <summary>
        /// Numer telefonu użytkownika (weryfikowany formatem).
        /// </summary>
        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Data urodzenia użytkownika.
        /// </summary>
        [Required(ErrorMessage = "Data urodzenia jest wymagana.")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Kraj pochodzenia użytkownika.
        /// </summary>
        [Required(ErrorMessage = "Kraj jest wymagany.")]
        public string Country { get; set; }
    }
}
