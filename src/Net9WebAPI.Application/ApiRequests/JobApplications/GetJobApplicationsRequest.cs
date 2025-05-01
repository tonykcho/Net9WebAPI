using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.Application.Mappers;
using Net9WebAPI.DataAccess.DbContexts;
using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.Application.ApiRequests.JobApplications;

public class GetJobApplicationsRequest : IApiRequest
{
}

public class GetJobApplicationsRequestHandler(Net9WebAPIDbContext dbContext, ILogger<GetJobApplicationsRequestHandler> logger) : IApiRequestHandler<GetJobApplicationsRequest>
{
    public async Task<IApiResult> Handle(GetJobApplicationsRequest request, CancellationToken cancellationToken)
    {
        IApiResult apiResult;

        try
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                logger.LogInformation("GetJobApplicationsRequestHandler started");

                var JobApplications = await dbContext.JobApplications.ToListAsync(cancellationToken);

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