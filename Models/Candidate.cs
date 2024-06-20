namespace OVS360SolutionsAPI.Models
{
    public class Candidate
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? RelevantExperience { get; set; }
        public string? Skills { get; set; }
        public string? ResumeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserSecurityId { get; set; }

    }
}
