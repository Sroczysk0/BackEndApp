namespace ConsumerComplaints.Core.Entities

{

    public class Complaint
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string Issue { get; set; }
        public string Company { get; set; }

        // 🔽 ZMIANA TU
        public string DateReceived { get; set; } // ← zamiast DateTime
    }


}