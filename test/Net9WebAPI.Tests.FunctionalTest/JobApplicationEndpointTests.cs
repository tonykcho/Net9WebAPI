using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Bogus;
using Net9WebAPI.Application.ApiRequests.JobApplications;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.Domain.Models;

namespace Net9WebAPI.Tests.FunctionalTest;

public class JobApplicationEndpointTests : IClassFixture<ApiTestFixture>
{
    private readonly ApiTestFixture _fixture;
    private readonly Faker<CreateJobApplicationRequest> _requestGenerator = new Faker<CreateJobApplicationRequest>()
        .RuleFor(x => x.JobTitle, f => f.Name.JobTitle())
        .RuleFor(x => x.CompanyName, f => f.Company.CompanyName())
        .RuleFor(x => x.ApplicationStatus, f => f.PickRandom<JobApplicationStatus>())
        .RuleFor(x => x.ApplicationDate, f => f.Date.RecentOffset());
    public JobApplicationEndpointTests(ApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateJobApplication_WithValidInput_ReturnOKResponse()
    {
        // Arrange
        var request = _requestGenerator.Generate();

        // Act
        var result = await _fixture.Client.PostAsJsonAsync("/api/JobApplications", request);
        var content = await result.Content.ReadFromJsonAsync<JobApplicationDto>();

        // Assert
        result.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);

        Assert.NotNull(content);
        Assert.Equal(request.JobTitle, content.JobTitle);
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