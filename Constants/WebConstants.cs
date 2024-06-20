namespace OVS360SolutionsAPI.Constants
{
    public static class WebConstants
    {
        // Configurations
        public const string ConfigurationFileName = "appsettings.json";
        public const string ConnectionName = "DefaultConnection";
        public const string ResumeDirectory = "Resumes";
        public const string failureMessage = "Unable to save the";
        public const string blankInput = "Input JSON is blank";
        public const string internalServerErrorMessage = "Internal server error!";
    }

    public enum Entity {
        enquiry,
        candidate,
    }
}
