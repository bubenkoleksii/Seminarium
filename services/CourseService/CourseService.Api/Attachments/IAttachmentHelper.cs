namespace CourseService.Api.Attachments;

public interface IAttachmentHelper
{
    public Either<List<AttachmentModelRequest>?, Error> ConvertToAttachmentRequests(IEnumerable<IFormFile>? formFiles);
}
