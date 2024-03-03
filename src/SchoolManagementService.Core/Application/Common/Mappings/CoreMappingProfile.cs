using SchoolManagementService.Core.Application.School.Commands.CreateSchool;
using SchoolManagementService.Core.Application.School.Models;

namespace SchoolManagementService.Core.Application.Common.Mappings;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        ConfigureSchoolMapping();
    }

    private void ConfigureSchoolMapping()
    {
        CreateMap<CreateSchoolCommand, Domain.School>();
        CreateMap<Domain.School, SchoolModelResponse>();
    }
}
