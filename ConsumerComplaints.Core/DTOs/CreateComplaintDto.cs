namespace ConsumerComplaints.Core.DTOs
{
    public class CreateComplaintDto
    {
        public string? Product { get; set; }
        public string? Issue { get; set; }
        public DateTime DateReceived { get; set; }
        public string? State { get; set; }
        public string? SubIssue { get; set; }

        public string? SubmittedVia { get; set; } // ✅ DODANE
    }
}