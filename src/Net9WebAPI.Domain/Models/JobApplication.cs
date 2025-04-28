using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.Domain.Models
{
    public enum JobApplicationStatus
    {
        Applied,
        Interviewing,
        Offered,
        Rejected
    }

    public class JobApplication : BaseModel
    {
        public required string JobTitle { get; set; }
        public required string CompanyName { get; set; }
        public JobApplicationStatus ApplicationStatus { get; set; }
        public DateTimeOffset ApplicationDate { get; set; }
    }
}