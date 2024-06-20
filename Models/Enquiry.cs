namespace OVS360SolutionsAPI.Models
{
    public partial class Enquiry
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
