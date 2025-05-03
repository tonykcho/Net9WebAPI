using Microsoft.EntityFrameworkCore;
using Net9WebAPI.DataAccess.DataModels;

namespace Net9WebAPI.DataAccess.ModelConfigurations;
public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.JobTitle)
            .IsRequired();

        builder.Property(x => x.CompanyName)
            .IsRequired();

        builder.Property(x => x.ApplicationStatus)
            .IsRequired();

        builder.Property(x => x.ApplicationDate)
            .IsRequired();
    }
}