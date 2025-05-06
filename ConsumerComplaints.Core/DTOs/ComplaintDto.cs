namespace ConsumerComplaints.Core.DTOs
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string Issue { get; set; }
        public string Company { get; set; }

        // 🔽 TU MUSI BYĆ string, tak jak w Complaint.cs
        public string DateReceived { get; set; }
    }
}