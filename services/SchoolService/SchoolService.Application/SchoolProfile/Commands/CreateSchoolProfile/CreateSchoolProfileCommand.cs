namespace SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;

public class CreateSchoolProfileCommand : IRequest<Either<SchoolProfileModelResponse, Error>>
{
    public required Guid UserId { get; set; }

    public required string InvitationCode { get; set; }

    public string? Phone { get; set; }

    public string? Details { get; set; }

    public string? TeachersExperience { get; set; }

    public string? TeachersEducation { get; set; }

    public string? TeachersQualification { get; set; }

    public uint? TeachersLessonsPerCycle { get; set; }

    public DateOnly? StudentsDateOfBirth { get; set; }

    public string? StudentsAptitudes { get; set; }

    public string? ParentsAddress { get; set; }

    public string? ParentsRelationshipToChild { get; set; }

}
