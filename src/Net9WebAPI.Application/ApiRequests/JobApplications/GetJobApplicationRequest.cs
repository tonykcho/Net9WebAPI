using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.Application.Mappers;
using Net9WebAPI.DataAccess.DbContexts;
using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.Application.ApiRequests.JobApplications;
public class GetJobApplicationRequest : IApiRequest
{
    public int Id { get; set; }
}

public class GetJobApplicationRequestHandler(Net9WebAPIDbContext dbContext) : IApiRequestHandler<GetJobApplicationRequest>
{
    public async Task<IApiResult> Handle(GetJobApplicationRequest request, CancellationToken cancellationToken)
    {
        var jobApplication = await dbContext.JobApplications.FindAsync(request.Id);

        if (jobApplication == null)
        {
            return new ResourceNotFoundApiResult();
        }

        JobApplicationDto jobApplicationDto = JobApplicationMapper.From(jobApplication);

        return new ApiContentResult<JobApplicationDto>(jobApplicationDto);
    }
}