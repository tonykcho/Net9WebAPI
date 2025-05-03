namespace Net9WebAPI.DataAccess.DataModels
{
    public enum JobApplicationStatus
    {
        Applied,
        Interviewing,
        Offered,
        Rejected
    }

    public class JobApplication : EntityBase
    {
        public required string JobTitle { get; set; }
        public required string CompanyName { get; set; }
        public JobApplicationStatus ApplicationStatus { get; set; }
        public DateTimeOffset ApplicationDate { get; set; }
    }
}