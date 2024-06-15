﻿namespace CourseService.Application.Course.Commands.DeleteCourseGroup;

public class DeleteCourseGroupCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    ICommandContext commandContext
    ) : IRequestHandler<DeleteCourseGroupCommand, Option<Error>>
{
    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Option<Error>> Handle(DeleteCourseGroupCommand request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
                      UserId: request.UserId,
                      AllowedProfileTypes: [Constants.Teacher, Constants.SchoolAdmin]
        );

        var retrievingActiveProfileResult =
            await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null)
            return new InvalidError("school_profile");

        var course = await _commandContext.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, CancellationToken.None);
        if (course is null)
            return new NotFoundByIdError(request.Id, "course");

        if (activeProfile.Type != Constants.Teacher || activeProfile.Type != Constants.SchoolAdmin)
            return new InvalidError("school_profile");

        if (activeProfile.Type == Constants.Teacher)
        {
            var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(course.Id, activeProfile.Id);
            if (teacherValidatingResult.IsSome)
                return (Error)teacherValidatingResult;
        }

        var courseGroup = await _commandContext.CourseGroups.FindAsync(request.Id, CancellationToken.None);
        if (courseGroup is null)
            return new NotFoundByIdError(request.Id, "course_group");

        try
        {
            _commandContext.CourseGroups.Remove(courseGroup);
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the course group with ID {@Id}.", request.Id);

            return new InvalidDatabaseOperationError("course_group");
        }

        return Option<Error>.None;
    }
}
