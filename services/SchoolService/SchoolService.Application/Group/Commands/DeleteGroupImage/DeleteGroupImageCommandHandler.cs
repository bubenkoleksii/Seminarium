namespace SchoolService.Application.Group.Commands.DeleteGroupImage;

public class DeleteGroupImageCommandHandler : IRequestHandler<DeleteGroupImageCommand, Option<Error>>
{
    public Task<Option<Error>> Handle(DeleteGroupImageCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
