namespace ConsumerComplaints.Core.DTOs
{
    public class CreateCommentDto
    {
        public string Content { get; set; } = string.Empty;
        public int ComplaintId { get; set; }
    }
}