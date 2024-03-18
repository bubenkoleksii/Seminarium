﻿namespace SchoolService.Application.School.Commands.ArchiveSchool;

public class ArchiveSchoolCommandValidator : AbstractValidator<ArchiveSchoolCommand>
{
    public ArchiveSchoolCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}