using System;
using System.ComponentModel.DataAnnotations;

namespace ConsumerComplaints.Core.DTOs
{
    /// <summary>
    /// Reprezentuje komentarz powiązany ze zgłoszeniem.
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// ID komentarza (dla wyświetlenia).
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// ID komentarza (do identyfikacji).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Treść komentarza (wymagana, max 500 znaków).
        /// </summary>
        [Required(ErrorMessage = "Treść komentarza jest wymagana.")]
        [MaxLength(500, ErrorMessage = "Komentarz może mieć maksymalnie 500 znaków.")]
        public string Content { get; set; }

        /// <summary>
        /// ID zgłoszenia, do którego przypisany jest komentarz.
        /// </summary>
        public int ComplaintId { get; set; }

        /// <summary>
        /// Nazwa autora komentarza (jeśli dostępna).
        /// </summary>
        public string? AuthorName { get; set; }

        /// <summary>
        /// Data utworzenia komentarza (UTC w formacie yyyy-MM-dd HH:mm:ss).
        /// </summary>
        public string? CreatedAt { get; set; }
    }
}