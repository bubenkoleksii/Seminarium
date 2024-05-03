namespace SchoolService.Application.School.Queries.GetOneSchool;

public class GetOneSchoolQueryHandler : IRequestHandler<GetOneSchoolQuery, Either<SchoolModelResponse, Error>>
{
    private readonly IQueryContext _queryContext;

    private readonly IMapper _mapper;

    private readonly IS3Service _s3Service;

    private readonly IOptions<S3Options> _s3Options;

    public GetOneSchoolQueryHandler(IQueryContext queryContext, IMapper mapper, IS3Service s3Service, IOptions<S3Options> s3Options)
    {
        _queryContext = queryContext;
        _mapper = mapper;
        _s3Service = s3Service;
        _s3Options = s3Options;
    }

    public async Task<Either<SchoolModelResponse, Error>> Handle(GetOneSchoolQuery request, CancellationToken cancellationToken)
    {
        var entity = await _queryContext.Schools
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "school");

        if (entity.Img is null)
        {
            var schoolResponseWithImgNull = _mapper.Map<SchoolModelResponse>(entity);
            return schoolResponseWithImgNull;
        }

        var image = GetImage(entity.Img);
        if (image.IsRight)
            return (Error)image;

        var fileSuccess = (FileSuccess)image;
        var schoolResponse = _mapper.Map<SchoolModelResponse>(entity) with { Img = fileSuccess.Url };
        return schoolResponse;
    }

    private Either<FileSuccess, Error> GetImage(string name)
    {
        var request = new GetFileRequest(name, _s3Options.Value.Bucket);
        var result = _s3Service.GetOne(request);

        if (result.IsLeft)
            return (FileSuccess)result;

        return (Error)result;
    }
}
