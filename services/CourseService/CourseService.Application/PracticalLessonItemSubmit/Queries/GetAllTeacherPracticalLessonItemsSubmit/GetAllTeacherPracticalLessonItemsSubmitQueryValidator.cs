namespace CourseService.Application.PracticalLessonItemSubmit.Queries.GetAllTeacherPracticalLessonItemsSubmit;

public class GetAllTeacherPracticalLessonItemsSubmitQueryValidator : AbstractValidator<GetAllTeacherPracticalLessonItemsSubmitQuery>
{
    public GetAllTeacherPracticalLessonItemsSubmitQueryValidator()
    {
        RuleFor(x => x.ItemId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
