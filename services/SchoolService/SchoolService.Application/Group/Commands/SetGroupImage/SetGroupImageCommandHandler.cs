namespace SchoolService.Application.Group.Commands.SetGroupImage;

public class SetGroupImageCommandHandler : IRequestHandler<SetGroupImageCommand, Either<FileSuccess, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IOptions<S3Options> _s3Options;

    private readonly IS3Service _s3Service;

    public SetGroupImageCommandHandler(ICommandContext commandContext, IOptions<S3Options> s3Options, IS3Service s3Service)
    {
        _commandContext = commandContext;
        _s3Options = s3Options;
        _s3Service = s3Service;
    }

    public Task<Either<FileSuccess, Error>> Handle(SetGroupImageCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
