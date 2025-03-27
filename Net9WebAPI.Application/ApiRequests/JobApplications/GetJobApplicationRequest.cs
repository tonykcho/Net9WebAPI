using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.Application.Mappers;
using Net9WebAPI.DataAccess.Abstract;

namespace Net9WebAPI.Application.ApiRequests.JobApplications;
public class GetJobApplicationRequest : IApiRequest
{
    public int Id { get; set; }
}

public class GetJobApplicationRequestHandler(IJobApplicationRepository jobApplicationRepository) : IApiRequestHandler<GetJobApplicationRequest>
{
    public async Task<IApiResult> Handle(GetJobApplicationRequest request, CancellationToken cancellationToken)
    {
        var jobApplication = await jobApplicationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (jobApplication == null)
        {
            return new ResourceNotFoundApiResult();
        }

        JobApplicationDto jobApplicationDto = JobApplicationMapper.From(jobApplication);

        return new ApiContentResult<JobApplicationDto>(jobApplicationDto);
    }
}