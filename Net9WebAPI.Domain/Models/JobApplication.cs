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
    
    public class JobApplication : BaseModel, IAggregateRoot
    {
        public required string JobTitle { get; set; }
        public required string CompanyName { get; set; }
        public JobApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}