using Shared.Contracts.SchoolProfile;

namespace Shared.Contracts.Group;

public class GroupContract
{
    public Guid Id { get; set; }

    public Guid SchoolId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public required string Name { get; set; }

    public byte StudyPeriodNumber { get; set; }

    public string? Img { get; set; }

    public IEnumerable<SchoolProfileContract>? Students { get; set; }

    public SchoolProfileContract? ClassTeacher { get; set; }
}
