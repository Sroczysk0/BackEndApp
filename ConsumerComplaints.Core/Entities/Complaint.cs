using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsumerComplaints.Core.Entities
{
    [Table("complaints")] // <-- to sprawia, że EF szuka tabeli o nazwie "complaints"
    public class Complaint
    {
        [Key]
        [Column("Complaint ID")]
        public int Id { get; set; }

        [Column("Submitted via")]
        public string? SubmittedVia { get; set; }

        [Column("Date received")]
        public DateTime DateReceived { get; set; }

        [Column("State")]
        public string? State { get; set; }

        [Column("Product")]
        public string? Product { get; set; }

        [Column("Issue")]
        public string? Issue { get; set; }

        [Column("Sub-issue")]
        public string? SubIssue { get; set; }
    }
}