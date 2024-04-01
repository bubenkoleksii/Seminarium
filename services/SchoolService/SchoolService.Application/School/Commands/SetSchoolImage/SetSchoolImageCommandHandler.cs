namespace SchoolService.Application.School.Commands.SetSchoolImage;

public class SetSchoolImageCommandHandler : IRequestHandler<SetSchoolImageCommand, Either<FileSuccess, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IOptions<S3Options> _s3Options;

    private readonly IS3Service _s3Service;

    public SetSchoolImageCommandHandler(ICommandContext commandContext, IOptions<S3Options> s3Options, IS3Service s3Service)
    {
        _commandContext = commandContext;
        _s3Options = s3Options;
        _s3Service = s3Service;
    }

    public async Task<Either<FileSuccess, Error>> Handle(SetSchoolImageCommand request, CancellationToken cancellationToken)
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

        var newFileName = $"{Guid.NewGuid()}__{request.Name}";
        var uploadingResult = await UploadNewImage(request.Stream, newFileName, request.UrlExpirationInMin);

        if (uploadingResult.IsLeft)
        {
            entity.Img = newFileName;

            try
            {
                await _commandContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "An error occurred while setting image the school with values {@SchoolId} {@FileName}.", entity.Id, request.Name);

                return new InvalidDatabaseOperationError("school");
            }
        }

        return uploadingResult;
    }

    private async Task<Either<FileSuccess, Error>> UploadNewImage(Stream stream, string fileName, int? urlExpirationInMin)
    {
        var newFileRequest = urlExpirationInMin != null
            ? new SaveFileRequest(stream, fileName, _s3Options.Value.Bucket, urlExpirationInMin.Value)
            : new SaveFileRequest(stream, fileName, _s3Options.Value.Bucket);

        return await _s3Service.UploadOne(newFileRequest);
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
