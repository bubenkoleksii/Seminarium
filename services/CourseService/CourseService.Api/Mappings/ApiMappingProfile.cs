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

        CreateMap<GetAllCoursesParams, GetAllCoursesQuery>();

        CreateMap<GetAllCoursesModelResponse, GetAllCoursesResponse>();
    }

    private void ConfigureLessonMapping()
    {
        CreateMap<CreateLessonRequest, CreateLessonCommand>();

        CreateMap<UpdateLessonRequest, UpdateLessonCommand>();

        CreateMap<LessonModelResponse, LessonResponse>();

        CreateMap<GetAllLessonsParams, GetAllLessonsQuery>();

        CreateMap<GetAllLessonsModelResponse, GetAllLessonsResponse>();
    }

    private void ConfigurePracticalLessonItemMapping()
    {
        CreateMap<CreatePracticalLessonItemRequest, CreatePracticalLessonItemCommand>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());

        CreateMap<UpdatePracticalLessonItemRequest, UpdatePracticalLessonItemCommand>();

        CreateMap<PracticalLessonItemModelResponse, PracticalLessonItemResponse>();
    }

    private void ConfigureTheoryLessonItemMapping()
    {
        CreateMap<CreateTheoryLessonItemRequest, CreateTheoryLessonItemCommand>()
           .ForMember(dest => dest.Attachments, opt => opt.Ignore());

        CreateMap<UpdateTheoryLessonItemRequest, UpdateTheoryLessonItemCommand>();

        CreateMap<TheoryLessonItemModelResponse, TheoryLessonItemResponse>();
    }

    private void ConfigurePracticalLessonItemSubmitMapping()
    {
        CreateMap<CreatePracticalLessonItemSubmitRequest, CreatePracticalLessonItemSubmitCommand>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());

        CreateMap<UpdatePracticalLessonItemSubmitRequest, UpdatePracticalLessonItemSubmitCommand>();

        CreateMap<PracticalLessonItemSubmitModelResponse, PracticalLessonItemSubmitResponse>();

        CreateMap<GetAllPracticalLessonItemSubmitModelResponse, GetAllPracticalLessonItemSubmitResponse>();

        CreateMap<GetAllPracticalLessonItemSubmitModelResponseItem, GetAllPracticalLessonItemSubmitResponseItem>();

        CreateMap<AddPracticalItemSubmitResultsRequest, AddResultsPracticalLessonItemSubmitCommand>();

        CreateMap<StudentPracticalLessonItemModelResponse, StudentPracticalLessonItemResponse>();

        CreateMap<GetAllStudentPracticalLessonItemsModelResponse, GetAllStudentPracticalLessonItemsResponse>();
    }

    private void ConfigureGlobalMappings()
    {
        CreateMap<Enum, string>().ConvertUsing(e => e.ToString().ToSnakeCase());
    }
}
