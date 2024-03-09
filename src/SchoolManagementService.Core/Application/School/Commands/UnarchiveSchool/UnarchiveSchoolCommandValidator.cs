﻿using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Core.Application.School.Commands.UnarchiveSchool;

public class UnarchiveSchoolCommandValidator : AbstractValidator<UnarchiveSchoolCommand>
{
    public UnarchiveSchoolCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithErrorCode(ErrorTitles.Common.Null)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorTitles.Common.Empty);
    }
}
