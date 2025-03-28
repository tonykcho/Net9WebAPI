using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Net9WebAPI.Application.ApiRequests.JobApplications;
using Net9WebAPI.Domain.Models;

namespace Net9WebAPI.Tests.FunctionalTest;

public class JobApplicationEndpointTests : IClassFixture<ApiTestFixture>
{
    private readonly ApiTestFixture _fixture;
    public JobApplicationEndpointTests(ApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateJobApplication_WithValidInput_ReturnOKResponse()
    {
        // Arrange
        var request = new CreateJobApplicationRequest
        {
            JobTitle = "Software Developer",
            CompanyName = "Net9",
            ApplicationStatus = JobApplicationStatus.Applied,
            ApplicationDate = DateTimeOffset.UtcNow
        };

        var requestJson = JsonSerializer.Serialize(request);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        // Act
        var result = await _fixture.Client.PostAsync("/api/JobApplications", content);

        // Assert
        result.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
    }

    [Fact]
    public async Task GetJobApplicationsRequest_ReturnJobApplicationsList()
    {
        var result = await _fixture.Client.GetAsync("/api/JobApplications");
        var content = await result.Content.ReadFromJsonAsync<IList<JobApplication>>();

        // Assert
        result.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

        if (content is not null)
        {
            Console.WriteLine(content.Count);
        }
    }
}