﻿using SchoolManagementService.Core.Application.Common.DataContext;
using SchoolManagementService.Core.Application.School.Models;
using SchoolManagementService.Core.Domain.Errors;
using SchoolManagementService.Core.Domain.Errors.NotFound;

namespace SchoolManagementService.Core.Application.School.Queries;

public class GetOneSchoolQueryHandler : IRequestHandler<GetOneSchoolQuery, Either<SchoolModelResponse, Error>>
{
    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    public GetOneSchoolQueryHandler(IQueryContext queryContext, IMapper mapper)
    {
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<Either<SchoolModelResponse, Error>> Handle(GetOneSchoolQuery request, CancellationToken cancellationToken)
    {
        var entity = await _queryContext.Schools
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
            return new NotFoundByIdError(request.Id, "school");

        var schoolResponse = _mapper.Map<SchoolModelResponse>(entity);
        return schoolResponse;
    }
}
