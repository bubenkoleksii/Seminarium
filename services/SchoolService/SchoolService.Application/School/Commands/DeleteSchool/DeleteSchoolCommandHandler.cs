namespace SchoolService.Application.School.Commands.DeleteSchool;

public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IOptions<S3Options> _s3Options;

    private readonly IS3Service _s3Service;

    public DeleteSchoolCommandHandler(ICommandContext commandContext, IOptions<S3Options> s3Options, IS3Service s3Service)
    {
        _commandContext = commandContext;
        _s3Options = s3Options;
        _s3Service = s3Service;
    }

    public async Task<Option<Error>> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(s => s.JoiningRequest)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Option<Error>.None;

        entity.JoiningRequest.School = null;
        await DeleteImageIfExists(entity.Img);

        _commandContext.Schools.Remove(entity);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while archiving the school with ID {@SchoolId}.", request.Id);

            return new InvalidDatabaseOperationError("school");
        }

        return Option<Error>.None;
    }

    private async Task DeleteImageIfExists(string? name)
    {
        if (name == null) return;

        var deletingRequest = new DeleteFileRequest(name, _s3Options.Value.Bucket);
        await _s3Service.DeleteOne(deletingRequest);
    }
}
