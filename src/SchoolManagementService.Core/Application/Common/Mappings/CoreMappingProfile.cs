using SchoolManagementService.Core.Application.School.Commands.CreateSchool;

namespace SchoolManagementService.Core.Application.Common.Mappings;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        CreateMap<CreateSchoolCommand, Domain.School>();
    }
}
