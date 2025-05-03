using Bogus;
using Microsoft.EntityFrameworkCore;
using Net9WebAPI.DataAccess.DataModels;
using Net9WebAPI.DataAccess.DbContexts;
using NSubstitute;

public abstract class UnitTestBase
{
    public UnitTestBase()
    {
        MockDbContext = Substitute.For<Net9WebAPIDbContext>();
        PopulateJobApplicationDbSet();
    }

    public Net9WebAPIDbContext MockDbContext { get; set; }
    private readonly Faker<JobApplication> _jobApplicationGenerator = new Faker<JobApplication>()
        .RuleFor(x => x.Id, f => f.IndexFaker)
        .RuleFor(x => x.Guid, f => Guid.NewGuid())
        .RuleFor(x => x.CreatedAt, f => f.Date.RecentOffset())
        .RuleFor(x => x.LastUpdatedAt, f => f.Date.RecentOffset())
        .RuleFor(x => x.JobTitle, f => f.Name.JobTitle())
        .RuleFor(x => x.CompanyName, f => f.Company.CompanyName())
        .RuleFor(x => x.ApplicationStatus, f => f.PickRandom<JobApplicationStatus>())
        .RuleFor(x => x.ApplicationDate, f => f.Date.RecentOffset());

    private void PopulateJobApplicationDbSet()
    {
        List<JobApplication> jobApplications =
        [
            _jobApplicationGenerator.Generate(),
            _jobApplicationGenerator.Generate(),
            _jobApplicationGenerator.Generate(),
            _jobApplicationGenerator.Generate(),
            _jobApplicationGenerator.Generate(),
        ];

        var mockDbSet = CreateMockDbSet(jobApplications);
        MockDbContext.JobApplications.Returns(mockDbSet);
    }

    private DbSet<T> CreateMockDbSet<T>(IEnumerable<T> data) where T : class
    {
        var queryableData = data.AsQueryable();
        var mockDbSet = Substitute.For<DbSet<T>, IQueryable<T>>();

        // Mock IQueryable methods
        ((IQueryable<T>)mockDbSet).Provider.Returns(queryableData.Provider);
        ((IQueryable<T>)mockDbSet).Expression.Returns(queryableData.Expression);
        ((IQueryable<T>)mockDbSet).ElementType.Returns(queryableData.ElementType);
        ((IQueryable<T>)mockDbSet).GetEnumerator().Returns(queryableData.GetEnumerator());

        return mockDbSet;
    }
}