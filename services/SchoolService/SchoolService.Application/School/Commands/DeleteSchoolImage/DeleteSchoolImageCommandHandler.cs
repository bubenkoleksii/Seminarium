namespace SchoolService.Application.School.Commands.DeleteSchoolImage;

public class DeleteSchoolImageCommandHandler : IRequestHandler<DeleteSchoolImageCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IOptions<S3Options> _s3Options;

    private readonly IS3Service _s3Service;

    public DeleteSchoolImageCommandHandler(ICommandContext commandContext, IOptions<S3Options> s3Options, IS3Service s3Service)
    {
        _commandContext = commandContext;
        _s3Options = s3Options;
        _s3Service = s3Service;
    }

    public async Task<Option<Error>> Handle(DeleteSchoolImageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.SchoolId, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.SchoolId, "school");

        var deletingResult = await DeleteImageIfExists(entity.Img);
        if (deletingResult.IsSome)
        {
            Log.Error("An error occurred while deleting image the school with values {@SchoolId} {@FileName}.", entity.Id, entity.Img);
            return (Error)deletingResult;
        }

        entity.Img = null;
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while setting image to null the school with id {@SchoolId}.", entity.Id);

            return new InvalidDatabaseOperationError("school");
        }

        return Option<Error>.None;
    }

    private async Task<Option<Error>> DeleteImageIfExists(string? name)
    {
        if (name == null)
            return Option<Error>.None;

        var deletingRequest = new DeleteFileRequest(name, _s3Options.Value.Bucket);
        var deletingResult = await _s3Service.DeleteOne(deletingRequest);

        if (deletingResult.IsSome)
            return (Error)deletingResult;

        return Option<Error>.None;
    }
}
