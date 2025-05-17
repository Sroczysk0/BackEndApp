namespace ConsumerComplaints.Core.DTOs
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public string? Product { get; set; }
        public string? Issue { get; set; }
        public DateTime DateReceived { get; set; }  // <-- zmiana!
        public string? State { get; set; }
        public string? SubIssue { get; set; }
    }
}