using System;
using System.ComponentModel.DataAnnotations;

namespace ConsumerComplaints.Core.DTOs
{
    /// <summary>
    /// Model DTO do tworzenia nowego zgłoszenia (complaint).
    /// </summary>
    public class CreateComplaintDto
    {
        /// <summary>
        /// Nazwa produktu, którego dotyczy zgłoszenie.
        /// </summary>
        [Required(ErrorMessage = "Pole 'Product' jest wymagane.")]
        public string Product { get; set; }

        /// <summary>
        /// Opis problemu zgłaszanego przez klienta.
        /// </summary>
        [Required(ErrorMessage = "Pole 'Issue' jest wymagane.")]
        public string Issue { get; set; }

        /// <summary>
        /// Stan USA, z którego pochodzi zgłoszenie.
        /// </summary>
        [Required(ErrorMessage = "Pole 'State' jest wymagane.")]
        public string State { get; set; }

        /// <summary>
        /// Dodatkowy opis problemu (Sub-issue), opcjonalny.
        /// </summary>
        public string? SubIssue { get; set; }

        /// <summary>
        /// Kanał, przez który zgłoszenie zostało przesłane (np. Web, Phone), opcjonalny.
        /// </summary>
        public string? SubmittedVia { get; set; }

        /// <summary>
        /// Data otrzymania zgłoszenia.
        /// </summary>
        [Required(ErrorMessage = "Pole 'DateReceived' jest wymagane.")]
        public DateTime DateReceived { get; set; }
    }
}