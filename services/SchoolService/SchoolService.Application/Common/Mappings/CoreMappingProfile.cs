using SchoolService.Application.JoiningRequest.Commands.CreateJoiningRequest;
using Profile = AutoMapper.Profile;

namespace SchoolService.Application.Common.Mappings;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        ConfigureSchoolMapping();

        ConfigureJoiningRequestMapping();
    }

    private void ConfigureSchoolMapping()
    {
        CreateMap<CreateSchoolCommand, Domain.Entities.School>();

        CreateMap<UpdateSchoolCommand, Domain.Entities.School>();

        CreateMap<Domain.Entities.School, SchoolModelResponse>();
    }

    private void ConfigureJoiningRequestMapping()
    {
        CreateMap<CreateJoiningRequestCommand, Domain.Entities.JoiningRequest>();

        CreateMap<Domain.Entities.JoiningRequest, JoiningRequestModelResponse>();
    }
}
