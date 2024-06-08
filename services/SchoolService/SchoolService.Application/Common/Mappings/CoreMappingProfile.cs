using Profile = AutoMapper.Profile;

namespace SchoolService.Application.Common.Mappings;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        ConfigureSchoolMapping();

        ConfigureJoiningRequestMapping();

        ConfigureSchoolProfileMappings();

        ConfigureGroupMappings();

        ConfigureStudyPeriodMappings();
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

        CreateMap<Domain.Entities.JoiningRequest, RejectJoiningRequestModelResponse>();
    }

    private void ConfigureSchoolProfileMappings()
    {
        CreateMap<CreateSchoolProfileCommand, Domain.Entities.SchoolProfile>();

        CreateMap<UpdateSchoolProfileCommand, Domain.Entities.SchoolProfile>();

        CreateMap<Domain.Entities.SchoolProfile, SchoolProfileModelResponse>();
    }

    private void ConfigureGroupMappings()
    {
        CreateMap<CreateGroupCommand, Domain.Entities.Group>();

        CreateMap<UpdateGroupCommand, Domain.Entities.Group>();

        CreateMap<Domain.Entities.Group, GroupModelResponse>();

        CreateMap<Domain.Entities.Group, OneGroupModelResponse>();
    }

    private void ConfigureStudyPeriodMappings()
    {
        CreateMap<CreateStudyPeriodCommand, Domain.Entities.StudyPeriod>();

        CreateMap<UpdateStudyPeriodCommand, Domain.Entities.StudyPeriod>();

        CreateMap<Domain.Entities.StudyPeriod, StudyPeriodModelResponse>();
    }
}
