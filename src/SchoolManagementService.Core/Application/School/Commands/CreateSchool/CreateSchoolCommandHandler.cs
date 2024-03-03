using SchoolManagementService.Core.Application.Common.DataContext;
using SchoolManagementService.Core.Application.School.Common;
using SchoolManagementService.Core.Application.School.Models;
using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Core.Application.School.Commands.CreateSchool;

public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Either<SchoolModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IMapper _mapper;

    public CreateSchoolCommandHandler(ICommandContext commandContext, IMapper mapper)
    {
        _commandContext = commandContext;
        _mapper = mapper;
    }

    public async Task<Either<SchoolModelResponse, Error>> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.School>(request);

        var isAlreadyExists = UniquenessChecker.GetErrorIfAlreadyExists(_commandContext, entity, out var error);
        if (isAlreadyExists)
            return error!;

        await _commandContext.Schools.AddAsync(entity, cancellationToken);
        await _commandContext.SaveChangesAsync(cancellationToken);

        var schoolResponse = _mapper.Map<SchoolModelResponse>(entity);
        return schoolResponse;
    }
}
