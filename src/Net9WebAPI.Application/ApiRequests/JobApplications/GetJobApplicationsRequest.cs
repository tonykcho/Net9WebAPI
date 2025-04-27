using System.Transactions;
using Microsoft.Extensions.Logging;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.Application.Mappers;
using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.Application.ApiRequests.JobApplications;

public class GetJobApplicationsRequest : IApiRequest
{
}

public class GetJobApplicationsRequestHandler : IApiRequestHandler<GetJobApplicationsRequest>
{
    private readonly IJobApplicationRepository jobApplicationRepository;
    private readonly ILogger<GetJobApplicationsRequestHandler> logger;

    public GetJobApplicationsRequestHandler(
        IJobApplicationRepository jobApplicationRepository,
        ILogger<GetJobApplicationsRequestHandler> logger)
    {
        this.jobApplicationRepository = jobApplicationRepository;
        this.logger = logger;
    }

    public async Task<IApiResult> Handle(GetJobApplicationsRequest request, CancellationToken cancellationToken)
    {
        IApiResult apiResult;

        try
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                logger.LogInformation("GetJobApplicationsRequestHandler started");

                var JobApplications = await jobApplicationRepository.ListAsync(cancellationToken);

                logger.LogInformation("There are {Count} job applications", JobApplications.Count);

                IList<JobApplicationDto> jobApplicationDtos = JobApplications.Select(JobApplicationMapper.From).ToList();

                apiResult = new ApiContentResult<IList<JobApplicationDto>>(jobApplicationDtos);

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Transaction failed: " + ex.Message);
            throw;
        }

        return apiResult;
    }
}