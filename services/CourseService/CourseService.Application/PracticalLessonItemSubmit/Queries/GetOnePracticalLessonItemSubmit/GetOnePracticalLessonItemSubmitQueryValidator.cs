namespace CourseService.Application.PracticalLessonItemSubmit.Queries.GetOnePracticalLessonItemSubmit;

public class GetOnePracticalLessonItemSubmitQueryValidator : AbstractValidator<GetOnePracticalLessonItemSubmitQuery>
{
    public GetOnePracticalLessonItemSubmitQueryValidator()
    {
        RuleFor(x => x.StudentId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);

        RuleFor(x => x.ItemId)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
