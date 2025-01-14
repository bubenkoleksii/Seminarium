﻿namespace SchoolService.Application.School.Commands.DeleteSchoolImage;

public class DeleteSchoolImageCommandHandler : IRequestHandler<DeleteSchoolImageCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IFilesManager _filesManager;

    public DeleteSchoolImageCommandHandler(
        ICommandContext commandContext,
        ISchoolProfileManager schoolProfileManager,
        IFilesManager filesManager)
    {
        _commandContext = commandContext;
        _schoolProfileManager = schoolProfileManager;
        _filesManager = filesManager;
    }

    public async Task<Option<Error>> Handle(DeleteSchoolImageCommand request, CancellationToken cancellationToken)
    {
        var validationError =
            await _schoolProfileManager.ValidateSchoolProfileBySchool(request.UserId, request.SchoolId);

        if (validationError.IsSome)
            return (Error)validationError;

        var entity = await _commandContext.Schools
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.SchoolId, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.SchoolId, "school");

        var deletingResult = await _filesManager.DeleteFileIfExists(entity.Img);
        if (deletingResult.IsSome)
        {
            Log.Error("An error occurred while deleting image for the school with values {@SchoolId} {@FileName}.", entity.Id, entity.Img);
            return (Error)deletingResult;
        }

        entity.Img = null;
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while setting image to null for the school with id {@SchoolId}.", entity.Id);

            return new InvalidDatabaseOperationError("school");
        }

        return Option<Error>.None;
    }
}
