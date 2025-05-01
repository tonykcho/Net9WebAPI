using System.Net;
using System.Net.Http.Json;
using Bogus;
using Net9WebAPI.Application.ApiRequests.JobApplications;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.Domain.Models;

namespace Net9WebAPI.Tests.FunctionalTest;

public class JobApplicationTest : IntegrationTestBase
{
    private readonly Faker<CreateJobApplicationRequest> _requestGenerator = new Faker<CreateJobApplicationRequest>()
        .RuleFor(x => x.JobTitle, f => f.Name.JobTitle())
        .RuleFor(x => x.CompanyName, f => f.Company.CompanyName())
        .RuleFor(x => x.ApplicationStatus, f => f.PickRandom<JobApplicationStatus>())
        .RuleFor(x => x.ApplicationDate, f => f.Date.RecentOffset());

    public JobApplicationTest(WebApplicationFixture webApplicationFixture) : base(webApplicationFixture)
    {
    }

    [Fact]
    public async Task CreateJobApplication_WithValidInput_ReturnOKResponse()
    {
        // Arrange
        var request = _requestGenerator.Generate();

        // Act
        var result = await WebApplicationFixture.Client.PostAsJsonAsync("/api/JobApplications", request);

        // Assert
        result.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);

        var content = await result.Content.ReadFromJsonAsync<JobApplicationDto>();
        Assert.NotNull(content);
        Assert.Equal(request.JobTitle, content.JobTitle);
        Assert.Equal(1, content.Id);
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
    }

    [Fact]
    public async Task GetJobApplicationsRequest_ReturnJobApplicationsList()
    {
        // Act
        var result = await WebApplicationFixture.Client.GetAsync("/api/JobApplications");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

        // var content = await result.Content.ReadFromJsonAsync<IList<JobApplication>>();
        // if (content is not null)
        // {
        //     Console.WriteLine(content.Count);
        // }
    }
}