using Microsoft.EntityFrameworkCore;
using Net9WebAPI.DataAccess.DbContexts;
using Net9WebAPI.Domain.Models;
using NSubstitute;

public abstract class UnitTestBase
{
    public UnitTestBase()
    {
        MockDbContext = Substitute.For<Net9WebAPIDbContext>();
        PopulateJobApplicationDbSet();
    }

    public Net9WebAPIDbContext MockDbContext { get; set; }

    private void PopulateJobApplicationDbSet()
    {
        List<JobApplication> jobApplications = new List<JobApplication>();

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