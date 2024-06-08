namespace SchoolService.Application.SchoolProfile.Models;

public record Invitation(
    Guid SourceId,

    SchoolProfileType Type,

    DateTime Expired
);
