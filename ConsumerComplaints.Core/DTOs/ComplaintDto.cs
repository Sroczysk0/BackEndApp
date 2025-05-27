using System;

namespace ConsumerComplaints.Core.DTOs
{
    /// <summary>
    /// Reprezentuje zgłoszenie (complaint) widoczne w interfejsie API.
    /// </summary>
    public class ComplaintDto
    {
        /// <summary>
        /// ID zgłoszenia.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Nazwa produktu, którego dotyczy zgłoszenie.
        /// </summary>
        public string? Product { get; set; }

        /// <summary>
        /// Opis problemu zgłaszanego przez klienta.
        /// </summary>
        public string? Issue { get; set; }

        /// <summary>
        /// Data otrzymania zgłoszenia (DateTime).
        /// </summary>
        public DateTime DateReceived { get; set; }

        /// <summary>
        /// Stan USA, z którego pochodzi zgłoszenie.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Dodatkowe informacje o problemie (Sub-issue).
        /// </summary>
        public string? SubIssue { get; set; }

        /// <summary>
        /// Kanał, przez który zgłoszenie zostało przesłane (np. Web, Phone).
        /// </summary>
        public string? SubmittedVia { get; set; }
    }
}