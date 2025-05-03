using Net9WebAPI.Application.Dtos;
using Net9WebAPI.DataAccess.DataModels;

namespace Net9WebAPI.Application.Mappers;

public static class JobApplicationMapper
{
    public static JobApplicationDto From(JobApplication jobApplication)
    {
        return new JobApplicationDto
        {
            Id = jobApplication.Id,
            JobTitle = jobApplication.JobTitle,
            CompanyName = jobApplication.CompanyName,
            ApplicationStatus = jobApplication.ApplicationStatus,
            ApplicationDate = jobApplication.ApplicationDate
        };
    }
}