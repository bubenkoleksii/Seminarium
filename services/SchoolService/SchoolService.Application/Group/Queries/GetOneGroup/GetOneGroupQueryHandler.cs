namespace SchoolService.Application.Group.Queries.GetOneGroup;

public class GetOneGroupQueryHandler : IRequestHandler<GetOneGroupQuery, Either<OneGroupModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetOneGroupQueryHandler(ISchoolProfileManager schoolProfileManager, IQueryContext queryContext, IMapper mapper)
    {
        _schoolProfileManager = schoolProfileManager;
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<Either<OneGroupModelResponse, Error>> Handle(GetOneGroupQuery request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null)
            return new InvalidError("school_profile");

        var group = await _queryContext.Groups
            .Include(g => g.School)
            .Include(g => g.ClassTeacher)
            .Include(g => g.Students)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken: cancellationToken);

        if (group is null)
            return new NotFoundByIdError(request.Id, "group");

        switch (profile.Type)
        {
            case SchoolProfileType.SchoolAdmin or SchoolProfileType.Teacher:
                {
                    var validationError =
                        await _schoolProfileManager.ValidateSchoolProfileBySchool(request.UserId, group.SchoolId);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.ClassTeacher or SchoolProfileType.Student:
                {
                    var validationError = await _schoolProfileManager.ValidateSchoolProfileByGroup(request.UserId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
        }

        var groupResponse = _mapper.Map<OneGroupModelResponse>(group);
        groupResponse.SchoolName = group.School.Name;

        return groupResponse;
    }
}
