namespace SchoolService.Infrastructure.Persistence.EntityTypeConfigurations;

public class JoiningRequestConfiguration : IEntityTypeConfiguration<JoiningRequest>
{
    public void Configure(EntityTypeBuilder<JoiningRequest> builder)
    {
        builder.HasKey(request => request.Id);

        builder.Property(request => request.Id).ValueGeneratedOnAdd();
        builder.Property(request => request.RequesterEmail).HasMaxLength(50);
        builder.Property(request => request.RequesterPhone).HasMaxLength(50);
        builder.Property(request => request.RequesterFullName).HasMaxLength(250);
        builder.Property(request => request.Name).HasMaxLength(250);
        builder.Property(request => request.ShortName).HasMaxLength(250);
        builder.Property(request => request.TerritorialCommunity).HasMaxLength(250);
        builder.Property(request => request.Address).HasMaxLength(250);
        builder.Property(request => request.Status).HasDefaultValue(JoiningRequestStatus.Created);

        builder.HasQueryFilter(request => !request.IsArchived);
    }
}
