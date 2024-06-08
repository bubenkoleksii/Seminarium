namespace SchoolService.Application.Common.DataContext;

public interface IContext
{
    DbSet<Domain.Entities.School> Schools { get; set; }

    DbSet<Domain.Entities.JoiningRequest> JoiningRequests { get; set; }

    DbSet<Domain.Entities.SchoolProfile> SchoolProfiles { get; set; }

    DbSet<Domain.Entities.Group> Groups { get; set; }

    DbSet<Domain.Entities.StudyPeriod> StudyPeriods { get; set; }

    DbSet<Domain.Entities.GroupNotice> GroupNotices { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
