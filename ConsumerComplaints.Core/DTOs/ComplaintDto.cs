namespace ConsumerComplaints.Core.DTOs

{

    public class ComplaintDto

    {

        public int Id { get; set; }               // Identyfikator skargi

        public string Product { get; set; }       // Nazwa produktu

        public string Issue { get; set; }         // Opis problemu

        public string Company { get; set; }       // Nazwa firmy

        public DateTime DateReceived { get; set; } // Data otrzymania skargi

    }

}