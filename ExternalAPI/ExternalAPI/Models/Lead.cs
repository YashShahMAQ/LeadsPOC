namespace ExternalAPI.Models
{
    public class Lead
    {
        public int Id { get; set; }
        public required string EmailAddress { get; set; }
        public required string ContactPerson { get; set; }
        public required string InterestArea { get; set; }
    }
}
