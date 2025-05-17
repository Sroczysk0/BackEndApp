namespace ConsumerComplaints.Core.DTOs
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public int ComplaintId { get; set; }
    }
}