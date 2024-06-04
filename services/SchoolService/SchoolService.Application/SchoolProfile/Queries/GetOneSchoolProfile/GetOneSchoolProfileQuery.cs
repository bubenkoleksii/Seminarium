namespace SchoolService.Application.SchoolProfile.Queries.GetOneSchoolProfile;

public record GetOneSchoolProfileQuery(
    Guid Id,

    Guid? UserId
) : IRequest<Either<SchoolProfileModelResponse, Error>>;
