namespace ConsumerComplaints.WebAPI.Dto
{
    public class CommentDto
    {
        public string Content { get; set; }
        public int ComplaintId { get; set; }
        
        public string AuthorName { get; set; } = "anonim";
        public string CreatedAt { get; set; }
    }
}