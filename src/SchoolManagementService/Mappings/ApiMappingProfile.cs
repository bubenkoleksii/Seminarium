using SchoolManagementService.Core.Application.School.Commands.CreateSchool;
using SchoolManagementService.Core.Application.School.Models;
using SchoolManagementService.Models.School;

namespace SchoolManagementService.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        ConfigureSchoolMapping();
    }

    private void ConfigureSchoolMapping()
    {
        CreateMap<CreateSchoolRequest, CreateSchoolCommand>();

        CreateMap<SchoolModelResponse, SchoolResponse>();
    }
}
