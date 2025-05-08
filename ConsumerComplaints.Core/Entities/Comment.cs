using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ConsumerComplaints.Infrastructure.Entities;

namespace ConsumerComplaints.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        // Do jakiej skargi należy komentarz
        public int ComplaintId { get; set; }
       
        [ForeignKey("ComplaintId")]
        public Complaint Complaint { get; set; }
        

        // Użytkownik (może być null jeśli anonimowy)
        public string? UserId { get; set; }
        public UserEntity? User { get; set; }

        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }
}