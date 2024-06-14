namespace CourseService.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        ConfigureGlobalMappings();

        ConfigureCourseMapping();
        ConfigureLessonMapping();
        ConfigurePracticalLessonItemMapping();
        ConfigureTheoryLessonItemMapping();
        ConfigurePracticalLessonItemSubmitMapping();
    }

    private void ConfigureCourseMapping()
    {
        CreateMap<CreateCourseRequest, CreateCourseCommand>();

        CreateMap<UpdateCourseRequest, UpdateCourseCommand>();

        CreateMap<CourseModelResponse, CourseResponse>();

        CreateMap<CourseTeacherModelResponse, CourseTeacherResponse>();

        CreateMap<CourseGroupModelResponse, CourseGroupResponse>();
    }

    private void ConfigureLessonMapping()
    {
        CreateMap<CreateLessonRequest, CreateLessonCommand>();

        CreateMap<UpdateLessonRequest, UpdateLessonCommand>();

        CreateMap<LessonModelResponse, LessonResponse>();
    }

    private void ConfigurePracticalLessonItemMapping()
    {
        CreateMap<CreatePracticalLessonItemRequest, CreatePracticalLessonItemCommand>();

        CreateMap<UpdatePracticalLessonItemRequest, UpdatePracticalLessonItemCommand>();

        CreateMap<PracticalLessonItemModelResponse, PracticalLessonItemResponse>();
    }

    private void ConfigureTheoryLessonItemMapping()
    {
        CreateMap<CreateTheoryLessonItemRequest, CreateTheoryLessonItemCommand>();

        CreateMap<UpdateTheoryLessonItemRequest, UpdateTheoryLessonItemCommand>();

        CreateMap<TheoryLessonItemModelResponse, TheoryLessonItemResponse>();
    }

    private void ConfigurePracticalLessonItemSubmitMapping()
    {
        CreateMap<CreatePracticalLessonItemSubmitRequest, CreatePracticalLessonItemSubmitCommand>();

        CreateMap<UpdatePracticalLessonItemSubmitRequest, UpdatePracticalLessonItemSubmitCommand>();

        CreateMap<PracticalLessonItemSubmitModelResponse, PracticalLessonItemSubmitResponse>();
    }

    private void ConfigureGlobalMappings()
    {
        CreateMap<Enum, string>().ConvertUsing(e => e.ToString().ToSnakeCase());
    }
}
