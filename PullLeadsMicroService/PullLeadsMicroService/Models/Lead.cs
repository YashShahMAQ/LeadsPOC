using Microsoft.EntityFrameworkCore;

namespace PullLeadsMicroService.Models
{
    [Index(nameof(EmailAddress), IsUnique = true)]
    public class Lead
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPerson { get; set; }
        public string InterestArea { get; set; }
    }
}
