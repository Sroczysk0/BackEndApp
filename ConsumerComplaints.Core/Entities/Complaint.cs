namespace ConsumerComplaints.Core.Entities

{

    public class Complaint

    {

        public int Id { get; set; }               // unikalny identyfikator

        public string Product { get; set; }       // produkt, którego dotyczy skarga

        public string Issue { get; set; }         // problem z produktem

        public string Company { get; set; }       // nazwa firmy

        public DateTime DateReceived { get; set; } // data otrzymania skargi

    }

}