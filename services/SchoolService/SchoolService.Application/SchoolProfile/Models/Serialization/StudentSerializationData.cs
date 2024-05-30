namespace SchoolService.Application.SchoolProfile.Models.Serialization;

public record StudentSerializationData(
    DateOnly? StudentDateOfBirth,

    string? StudentAptitudes,

    bool StudentIsClassLeader,

    bool StudentIsIndividually,

    string StudentHealthGroup
);
