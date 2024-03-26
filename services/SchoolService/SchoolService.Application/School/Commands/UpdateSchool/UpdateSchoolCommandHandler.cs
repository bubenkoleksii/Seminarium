using SchoolService.Application.Common.Extensions;

namespace SchoolService.Application.School.Commands.UpdateSchool;

public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, Either<SchoolModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IRequestClient<SaveFileCommand> _saveFileClient;

    private readonly IMapper _mapper;

    public UpdateSchoolCommandHandler(ICommandContext commandContext, IRequestClient<SaveFileCommand> saveFileClient, IMapper mapper)
    {
        _commandContext = commandContext;
        _saveFileClient = saveFileClient;
        _mapper = mapper;
    }

    public async Task<Either<SchoolModelResponse, Error>> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "school");

        _mapper.Map(request, entity);

        _commandContext.Schools.Update(entity);

        if (request.Img is not null)
        {
            var command = new SaveFileCommand(request.Img.ToArray(), "school", "file.jpg", Guid.NewGuid());
            var response = await _saveFileClient.GetResponse<SaveFileSuccess>(command, cancellationToken);

            var result = response.Message;
            Log.Information(result.Name);
        }

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the school with values {@Request}.", request);

            return new InvalidDatabaseOperationError("school");
        }

        var schoolResponse = _mapper.Map<SchoolModelResponse>(entity);
        return schoolResponse;
    }
}
