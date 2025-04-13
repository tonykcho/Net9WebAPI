using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.ApiRequests.JobApplications;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.DataAccess.Abstract;
using Net9WebAPI.Domain.Models;
using NSubstitute;

namespace Net9WebAPI.Tests.UnitTest.JobApplications;

public class CreateJobApplicationRequestHandlerUnitTest
{
    [Fact]
    public async Task CreateJobApplication_WithValidInput_ReturnNoContentAPIResult()
    {
        // Arrange
        var mockJobApplicationRepositories = Substitute.For<IJobApplicationRepository>();

        mockJobApplicationRepositories.AddAsync(Arg.Any<JobApplication>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        mockJobApplicationRepositories.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(true);

        var cancellationToken = new CancellationToken();

        var mockRequest = new CreateJobApplicationRequest
        {
            JobTitle = "Software Developer",
            CompanyName = "Net9",
            ApplicationStatus = JobApplicationStatus.Applied,
            ApplicationDate = DateTimeOffset.UtcNow
        };
        var handler = new CreateJobApplicationRequestHandler(mockJobApplicationRepositories);

        // Act
        var result = await handler.Handle(mockRequest, cancellationToken);

        // Assert
        Assert.IsType<ApiContentResult<JobApplicationDto>>(result);
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
    }
}