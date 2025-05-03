using FluentValidation;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.Application.Mappers;
using Net9WebAPI.DataAccess.DataModels;
using Net9WebAPI.DataAccess.DbContexts;

namespace Net9WebAPI.Application.ApiRequests.JobApplications;

public class CreateJobApplicationRequest : IApiRequest
{
    public string? JobTitle { get; set; }
    public string? CompanyName { get; set; }
    public JobApplicationStatus ApplicationStatus { get; set; }
    public DateTimeOffset ApplicationDate { get; set; }

    public class Validator : AbstractValidator<CreateJobApplicationRequest>
    {
        public Validator()
        {
            RuleFor(request => request.JobTitle)
                .NotEmpty()
                .WithMessage("Job title is required.");

            RuleFor(request => request.CompanyName)
                .NotEmpty()
                .WithMessage("Company name is required.");

            RuleFor(request => request.ApplicationStatus)
                .IsInEnum()
                .WithMessage("Invalid application status.");

            RuleFor(request => request.ApplicationDate)
                .NotEqual(default(DateTimeOffset))
                .WithMessage("Application date is required.");
        }
    }
}

public class CreateJobApplicationRequestHandler(Net9WebAPIDbContext dbContext) : IApiRequestHandler<CreateJobApplicationRequest>
{
    public async Task<IApiResult> Handle(CreateJobApplicationRequest request, CancellationToken cancellationToken)
    {
        JobApplication jobApplication = new JobApplication
        {
            JobTitle = request.JobTitle ?? string.Empty,
            CompanyName = request.CompanyName ?? string.Empty,
            ApplicationStatus = request.ApplicationStatus,
            ApplicationDate = request.ApplicationDate.ToUniversalTime()
        };

        await dbContext.JobApplications.AddAsync(jobApplication, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        JobApplicationDto jobApplicationDto = JobApplicationMapper.From(jobApplication);

        return new ApiContentResult<JobApplicationDto>(jobApplicationDto);
    }
}