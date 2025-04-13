using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.Application.Mappers;
using Net9WebAPI.DataAccess.Abstract;

namespace Net9WebAPI.Application.ApiRequests.JobApplications;

public class GetJobApplicationsRequest : IApiRequest
{
}

public class GetJobApplicationsRequestHandler(IJobApplicationRepository jobApplicationRepository) : IApiRequestHandler<GetJobApplicationsRequest>
{
    public async Task<IApiResult> Handle(GetJobApplicationsRequest request, CancellationToken cancellationToken)
    {
        var JobApplications = await jobApplicationRepository.ListAsync(cancellationToken);

        IList<JobApplicationDto> jobApplicationDtos = JobApplications.Select(JobApplicationMapper.From).ToList();

        return new ApiContentResult<IList<JobApplicationDto>>(jobApplicationDtos);
    }
}