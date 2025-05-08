namespace ConsumerComplaints.Core.DTOs
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public string? Product { get; set; }
        public string? Issue { get; set; }
        public string? DateReceived { get; set; }
        public string? State { get; set; }
        public string? SubIssue { get; set; }
    }
}