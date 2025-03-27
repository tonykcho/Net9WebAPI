using Net9WebAPI.DataAccess.Abstract;
using Net9WebAPI.DataAccess.DbContexts;
using Net9WebAPI.Domain.Models;

namespace Net9WebAPI.DataAccess.Repositories;

public class JobApplicationRepository(Net9WebAPIDbContext dbContext) : BaseRepository<JobApplication>(dbContext), IJobApplicationRepository
{
}