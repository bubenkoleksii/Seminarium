namespace SchoolService.Api.Models.SchoolProfile;

public class UpdateSchoolProfileRequest
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Details { get; set; }

    public string? TeacherExperience { get; set; }

    public string? TeacherEducation { get; set; }

    public string? TeacherQualification { get; set; }

    public uint? TeacherLessonsPerCycle { get; set; }

    public DateOnly? StudentDateOfBirth { get; set; }

    public string? StudentAptitudes { get; set; }

    public bool? StudentIsClassLeader { get; set; }

    public bool? StudentIsIndividually { get; set; }

    public string? StudentHealthGroup { get; set; }

    public string? ParentAddress { get; set; }
}
