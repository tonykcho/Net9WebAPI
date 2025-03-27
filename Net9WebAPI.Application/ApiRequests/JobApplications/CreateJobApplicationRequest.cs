using FluentValidation;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.DataAccess.Abstract;
using Net9WebAPI.Domain.Models;

namespace Net9WebAPI.Application.ApiRequests.JobApplications;

public class CreateJobApplicationRequest : IApiRequest
{
    public string? JobTitle { get; set; }
    public string? CompanyName { get; set; }
    public JobApplicationStatus ApplicationStatus { get; set; }
    public DateTime ApplicationDate { get; set; }

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
                .NotEqual(default(DateTime))
                .WithMessage("Application date is required.");
        }
    }
}

public class CreateJobApplicationRequestHandler(IJobApplicationRepository jobApplicationRepository) : IApiRequestHandler<CreateJobApplicationRequest>
{
    public async Task<IApiResult> Handle(CreateJobApplicationRequest request, CancellationToken cancellationToken)
    {
        JobApplication jobApplication = new JobApplication
        {
            JobTitle = request.JobTitle ?? string.Empty,
            CompanyName = request.CompanyName ?? string.Empty,
            ApplicationStatus = request.ApplicationStatus,
            ApplicationDate = request.ApplicationDate
        };

        await jobApplicationRepository.AddAsync(jobApplication, cancellationToken);

        await jobApplicationRepository.SaveChangesAsync(cancellationToken);

        return new NoContentApiResult();
    }
}