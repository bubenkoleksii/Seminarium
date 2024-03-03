using SchoolManagementService.Core.Application.School.Commands.CreateSchool;
using SchoolManagementService.Core.Application.School.Models;
using SchoolManagementService.Core.Domain.Enums.School;
using SchoolManagementService.Mappings.Parsers;
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
        CreateMap<CreateSchoolRequest, CreateSchoolCommand>()
            .ForMember(command => command.Type,
                options => options.MapFrom(request => request.Type.ParseToEnum<SchoolType>()))
            .ForMember(command => command.OwnershipType,
                options => options.MapFrom(request => request.OwnershipType.ParseToEnum<SchoolOwnershipType>()))
            .ForMember(command => command.Region,
                options => options.MapFrom(request => request.Region.ParseToEnum<SchoolRegion>()));

        CreateMap<SchoolModelResponse, SchoolResponse>()
            .ForMember(response => response.Type,
                options => options.MapFrom(modelResponse => modelResponse.Type.ParseToString()))
            .ForMember(response => response.OwnershipType,
                options => options.MapFrom(modelResponse => modelResponse.OwnershipType.ParseToString()))
            .ForMember(response => response.Region,
                options => options.MapFrom(modelResponse => modelResponse.Region.ParseToString()));
    }
}
