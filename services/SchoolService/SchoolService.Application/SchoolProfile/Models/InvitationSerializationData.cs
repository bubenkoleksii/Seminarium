using SchoolService.Domain.Enums.SchoolProfile;

namespace SchoolService.Application.SchoolProfile.Models;

public record InvitationSerializationData(
    Guid SourceId,

    SchoolProfileType Type,

    DateTime Expired
);
