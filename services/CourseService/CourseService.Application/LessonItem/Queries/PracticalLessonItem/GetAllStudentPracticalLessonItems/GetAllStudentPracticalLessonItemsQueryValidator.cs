namespace CourseService.Application.LessonItem.Queries.PracticalLessonItem.GetAllStudentPracticalLessonItems;

public class GetAllStudentPracticalLessonItemsQueryValidator : AbstractValidator<GetAllStudentPracticalLessonItemsQuery>
{
    public GetAllStudentPracticalLessonItemsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.StudentId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
