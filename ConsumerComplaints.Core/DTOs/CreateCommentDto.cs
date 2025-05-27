using System.ComponentModel.DataAnnotations;

namespace ConsumerComplaints.Core.DTOs
{
    /// <summary>
    /// Model DTO do tworzenia nowego komentarza.
    /// </summary>
    public class CreateCommentDto
    {
        /// <summary>
        /// Treść komentarza.
        /// Musi zawierać co najmniej 5 i maksymalnie 500 znaków.
        /// </summary>
        [Required(ErrorMessage = "Treść komentarza jest wymagana.")]
        [MinLength(5, ErrorMessage = "Komentarz musi mieć co najmniej 5 znaków.")]
        [MaxLength(500, ErrorMessage = "Komentarz może mieć maksymalnie 500 znaków.")]
        public string Content { get; set; }

        /// <summary>
        /// ID zgłoszenia (Complaint), do którego komentarz ma być przypisany.
        /// </summary>
        [Required(ErrorMessage = "ID zgłoszenia jest wymagane.")]
        public int ComplaintId { get; set; }
    }
}