using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsumerComplaints.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        // Relacja do skargi
        public int ComplaintId { get; set; }

        [ForeignKey("ComplaintId")]
        public Complaint Complaint { get; set; } = null!;

        // Relacja do użytkownika
        public string? UserId { get; set; }
        public UserEntity? User { get; set; }

        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }
}