namespace OVS360SolutionsAPI.Models
{
    public class Job
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ClientId { get; set; }
        public bool? IsActive { get; set; }

    }
}
