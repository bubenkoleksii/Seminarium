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
        CreateMap<CreatePracticalLessonItemCommand, PracticalLessonItem>()
            .ForMember(dest => dest.Deadline, opt => opt.MapFrom(src => src.Deadline.HasValue
                ? src.Deadline.Value.ToUniversalTime()
                : (DateTime?)null))
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());

        CreateMap<UpdatePracticalLessonItemCommand, PracticalLessonItem>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());

        CreateMap<PracticalLessonItem, PracticalLessonItemModelResponse>()
            .ForMember(dest => dest.Author, opt => opt.Ignore())
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
    }

    private void ConfigureTheoryLessonItemMapping()
    {
        CreateMap<CreateTheoryLessonItemCommand, Domain.Entities.TheoryLessonItem>()
             .ForMember(dest => dest.Deadline, opt => opt.MapFrom(src => src.Deadline.HasValue
                ? src.Deadline.Value.ToUniversalTime()
                : (DateTime?)null))
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());

        CreateMap<UpdateTheoryLessonItemCommand, TheoryLessonItem>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());

        CreateMap<TheoryLessonItem, TheoryLessonItemModelResponse>()
            .ForMember(dest => dest.Author, opt => opt.Ignore())
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
    }

    private void ConfigurePracticalLessonItemSubmitMapping()
    {
        CreateMap<CreatePracticalLessonItemSubmitCommand, Domain.Entities.PracticalLessonItemSubmit>();

        CreateMap<UpdatePracticalLessonItemSubmitCommand, Domain.Entities.PracticalLessonItemSubmit>();

        CreateMap<Domain.Entities.PracticalLessonItemSubmit, PracticalLessonItemSubmitModelResponse>();
    }
}
