using Profile = AutoMapper.Profile;

namespace CourseService.Application.Common.Mappings;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        ConfigureCourseMapping();
    }

    private void ConfigureCourseMapping()
    {
        CreateMap<CreateCourseCommand, Domain.Entities.Course>();

        CreateMap<Domain.Entities.Course, CourseModelResponse>();
    }
}
