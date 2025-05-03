using Net9WebAPI.DataAccess.DataModels;

namespace Net9WebAPI.Application.Dtos;

public class JobApplicationDto
{
    public int Id { get; set; }
    public required string JobTitle { get; set; }
    public required string CompanyName { get; set; }
    public JobApplicationStatus ApplicationStatus { get; set; }
    public DateTimeOffset ApplicationDate { get; set; }
}