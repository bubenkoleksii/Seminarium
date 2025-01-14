﻿namespace CourseService.Api.Attachments;

public class AttachmentHelper(IOptions<Shared.Contracts.Options.FileOptions> fileOptions)
    : IAttachmentHelper
{
    public Either<List<AttachmentModelRequest>?, Error> ConvertToAttachmentRequests(IEnumerable<IFormFile>? formFiles)
    {
        if (formFiles == null || !formFiles.Any())
            return new List<AttachmentModelRequest>();

        var maxAllowedSizeInMb = fileOptions.Value.MaxSizeInMb;

        var attachmentRequests = new List<AttachmentModelRequest>();
        foreach (var file in formFiles)
        {
            var mappingStreamResult = FileMapper.GetStreamIfValid(file, isImage: false, maxAllowedSizeInMb);

            if (mappingStreamResult.IsRight)
                return (Error)mappingStreamResult;

            var stream = (Stream)mappingStreamResult;

            var attachment = new AttachmentModelRequest { Name = file.FileName, Stream = stream };
            attachmentRequests.Add(attachment);
        }

        return attachmentRequests;
    }
}
