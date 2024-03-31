namespace SchoolService.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        ConfigureGlobalMappings();

        ConfigureSchoolMapping();
    }

    private void ConfigureSchoolMapping()
    {
        CreateMap<CreateSchoolRequest, CreateSchoolCommand>()
            .ForMember(command => command.Email, act => act.MapFrom(req => req.Email == null ? null : req.Email.ToLower()));

        CreateMap<UpdateSchoolRequest, UpdateSchoolCommand>()
            .ForMember(command => command.Email, act => act.MapFrom(req => req.Email == null ? null : req.Email.ToLower()));

        CreateMap<SchoolModelResponse, SchoolResponse>();
    }
    private void ConfigureGlobalMappings()
    {
        CreateMap<Enum, string>().ConvertUsing(e => e.ToString().ToSnakeCase());
    }
}
