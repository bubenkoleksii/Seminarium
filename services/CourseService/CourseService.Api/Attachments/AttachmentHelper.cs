namespace CourseService.Api.Attachments;

public class AttachmentHelper(IOptions<Shared.Contracts.Options.FileOptions> fileOptions)
    : IAttachmentHelper
{
    public Either<ICollection<AttachmentModelRequest>, Error> ConvertToAttachmentRequests(IEnumerable<IFormFile>? formFiles)
    {
        if (formFiles == null || !formFiles.Any())
            return [];

        var maxAllowedSizeInMb = fileOptions.Value.MaxSizeInMb;

        var attachmentRequests = new List<AttachmentModelRequest>();
        foreach (var file in formFiles)
        {
            var mappingStreamResult = FileMapper.GetStreamIfValid(file, isImage: false, maxAllowedSizeInMb);

            if (mappingStreamResult.IsRight)
                return (Error)mappingStreamResult;

            var stream = (Stream)mappingStreamResult;

            var attachment = new AttachmentModelRequest { Name = file.Name, Stream = stream };
            attachmentRequests.Add(attachment);
        }

        return attachmentRequests;
    }
}
