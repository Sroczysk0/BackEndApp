using System.ComponentModel.DataAnnotations.Schema;

namespace ConsumerComplaints.Core.Entities
{
    public class Complaint
    {
        [Column("Complaint ID")]
        public int Id { get; set; }

        [Column("Product")]
        public string? Product { get; set; }

        [Column("Issue")]
        public string? Issue { get; set; }

        [Column("Date received")]
        public DateTime DateReceived { get; set; }

        [Column("State")]
        public string? State { get; set; }

        [Column("Sub-issue")]
        public string? SubIssue { get; set; }

        public string? UserId { get; set; } // <-- DODAJ TO

        [ForeignKey("UserId")]
        public UserEntity? User { get; set; } // <-- DODAJ TO
        
        // Complaint.cs
        public string? SubmittedVia { get; set; }

    }
}