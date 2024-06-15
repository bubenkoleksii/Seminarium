using Profile = AutoMapper.Profile;

namespace CourseService.Application.Common.Mappings;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        ConfigureCourseMapping();

        ConfigureLessonMapping();

        ConfigurePracticalLessonItemMapping();

        ConfigureTheoryLessonItemMapping();

        ConfigurePracticalLessonItemSubmitMapping();
    }

    private void ConfigureCourseMapping()
    {
        CreateMap<CreateCourseCommand, Domain.Entities.Course>();

        CreateMap<UpdateCourseCommand, Domain.Entities.Course>();

        CreateMap<Domain.Entities.Course, CourseModelResponse>()
                  .ForMember(dest => dest.Teachers, opt => opt.Ignore())
                  .ForMember(dest => dest.Groups, opt => opt.Ignore());
        CreateMap<CourseGroup, CourseGroupModelResponse>();

        CreateMap<CourseTeacher, CourseTeacherModelResponse>();
    }

    private void ConfigureLessonMapping()
    {
        CreateMap<CreateLessonCommand, Domain.Entities.Lesson>();

        CreateMap<UpdateLessonCommand, Domain.Entities.Lesson>();

        CreateMap<Domain.Entities.Lesson, LessonModelResponse>();
    }

    private void ConfigurePracticalLessonItemMapping()
    {
        CreateMap<CreatePracticalLessonItemCommand, PracticalLessonItem>();

        CreateMap<UpdatePracticalLessonItemCommand, PracticalLessonItem>();

        CreateMap<PracticalLessonItem, PracticalLessonItemModelResponse>();
    }

    private void ConfigureTheoryLessonItemMapping()
    {
        CreateMap<CreateTheoryLessonItemCommand, Domain.Entities.TheoryLessonItem>();

        CreateMap<UpdateTheoryLessonItemCommand, TheoryLessonItem>();

        CreateMap<TheoryLessonItem, TheoryLessonItemModelResponse>();
    }

    private void ConfigurePracticalLessonItemSubmitMapping()
    {
        CreateMap<CreatePracticalLessonItemSubmitCommand, Domain.Entities.PracticalLessonItemSubmit>();

        CreateMap<UpdatePracticalLessonItemSubmitCommand, Domain.Entities.PracticalLessonItemSubmit>();

        CreateMap<Domain.Entities.PracticalLessonItemSubmit, PracticalLessonItemSubmitModelResponse>();
    }
}
