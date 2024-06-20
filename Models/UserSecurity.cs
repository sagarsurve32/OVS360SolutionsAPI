namespace OVS360SolutionsAPI.Models
{
    public class UserSecurity
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public int? OTP { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
