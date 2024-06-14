namespace CourseService.Api.Attachments;

public interface IAttachmentHelper
{
    public Either<ICollection<AttachmentModelRequest>, Error> ConvertToAttachmentRequests(IEnumerable<IFormFile>? formFiles);
}
