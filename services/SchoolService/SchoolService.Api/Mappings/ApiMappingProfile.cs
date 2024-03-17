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
        CreateMap<CreateSchoolRequest, CreateSchoolCommand>();

        CreateMap<UpdateSchoolRequest, UpdateSchoolCommand>();

        CreateMap<SchoolModelResponse, SchoolResponse>();
    }
    private void ConfigureGlobalMappings()
    {
        CreateMap<Enum, string>().ConvertUsing(e => e.ToString().ToSnakeCase());

        CreateMap<IFormFile, Stream?>().ConvertUsing(_ => null);
    }
}
