using Microsoft.EntityFrameworkCore;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.ApiRequests.JobApplications;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.DataAccess.DbContexts;
using Net9WebAPI.Domain.Abstract;
using Net9WebAPI.Domain.Models;
using NSubstitute;

namespace Net9WebAPI.Tests.UnitTest.JobApplications;

public class CreateJobApplicationRequestHandlerUnitTest : UnitTestBase
{
    [Fact]
    public async Task CreateJobApplication_WithValidInput_ReturnNoContentAPIResult()
    {
        var handler = new CreateJobApplicationRequestHandler(MockDbContext);

        var cancellationToken = new CancellationToken();

        var mockRequest = new CreateJobApplicationRequest
        {
            JobTitle = "Software Developer",
            CompanyName = "Net9",
            ApplicationStatus = JobApplicationStatus.Applied,
            ApplicationDate = DateTimeOffset.UtcNow
        };

        // Act
        var result = await handler.Handle(mockRequest, cancellationToken);

        // Assert
        Assert.IsType<ApiContentResult<JobApplicationDto>>(result);
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
    }
}