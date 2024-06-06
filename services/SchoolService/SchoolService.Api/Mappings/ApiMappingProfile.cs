namespace SchoolService.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        ConfigureGlobalMappings();

        ConfigureSchoolMapping();

        ConfigureJoiningRequestMapping();

        ConfigureSchoolProfileMappings();

        ConfigureGroupMappings();
    }

    private void ConfigureSchoolMapping()
    {
        CreateMap<CreateSchoolRequest, CreateSchoolCommand>();

        CreateMap<UpdateSchoolRequest, UpdateSchoolCommand>()
            .ForMember(command => command.Email, act => act.MapFrom(req => req.Email == null ? null : req.Email.ToLower()));

        CreateMap<SchoolModelResponse, SchoolResponse>();

        CreateMap<GetAllSchoolsParams, GetAllSchoolsQuery>();

        CreateMap<GetAllSchoolsModelResponse, GetAllSchoolsResponse>();
    }

    private void ConfigureJoiningRequestMapping()
    {
        CreateMap<CreateJoiningRequest, CreateJoiningRequestCommand>()
            .ForMember(command => command.RequesterEmail, act => act.MapFrom(req => req.RequesterEmail.ToLower()));

        CreateMap<JoiningRequestModelResponse, JoiningRequestResponse>();

        CreateMap<GetAllJoiningRequestsParams, GetAllJoiningRequestsQuery>();

        CreateMap<GetAllJoiningRequestsModelResponse, GetAllJoiningRequestsResponse>();

        CreateMap<RejectJoiningRequestModelResponse, RejectJoiningRequestResponse>();
    }

    private void ConfigureSchoolProfileMappings()
    {
        CreateMap<CreateSchoolProfileRequest, CreateSchoolProfileCommand>();

        CreateMap<UpdateSchoolProfileRequest, UpdateSchoolProfileCommand>()
            .ForMember(command => command.Email, act => act.MapFrom(req => req.Email == null ? null : req.Email.ToLower()));

        CreateMap<SchoolProfileModelResponse, SchoolProfileResponse>();
    }

    private void ConfigureGroupMappings()
    {
        CreateMap<CreateGroupRequest, CreateGroupCommand>();

        CreateMap<UpdateGroupRequest, UpdateGroupCommand>();

        CreateMap<GroupModelResponse, GroupResponse>();

        CreateMap<GetAllGroupsParams, GetAllGroupsQuery>();

        CreateMap<GetAllGroupsModelResponse, GetAllGroupsResponse>();

        CreateMap<OneGroupModelResponse, OneGroupResponse>();
    }

    private void ConfigureGlobalMappings()
    {
        CreateMap<Enum, string>().ConvertUsing(e => e.ToString().ToSnakeCase());
    }
}
