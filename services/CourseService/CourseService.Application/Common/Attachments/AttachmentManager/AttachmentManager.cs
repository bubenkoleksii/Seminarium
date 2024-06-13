namespace CourseService.Application.Common.Attachments.AttachmentManager;

public class AttachmentManager(IFilesManager filesManager) : IAttachmentManager
{
    private readonly IFilesManager _filesManager = filesManager;

    public async Task<(List<Attachment> Attachments, List<string> AttachmentsLinks)> ProcessAttachments
        (IEnumerable<AttachmentModelRequest>? attachmentRequests, Domain.Entities.LessonItem entity, string attachmentType)
    {
        var attachments = new List<Attachment>();
        var attachmentsLinks = new List<string>();

        if (attachmentRequests == null)
            return (attachments, attachmentsLinks);

        foreach (var attachmentRequest in attachmentRequests)
        {
            var newFileName = $"{Guid.NewGuid()}_{attachmentType}_{attachmentRequest.Name}";

            var uploadingResult = await _filesManager.UploadFile(attachmentRequest.Stream, newFileName, null);
            if (uploadingResult.IsRight)
                continue;

            var fileSuccess = (FileSuccess)uploadingResult;

            var attachment = new Attachment { Url = fileSuccess.Name, LessonItem = entity };
            attachments.Add(attachment);

            attachmentsLinks.Add(fileSuccess.Url);
        }

        return (attachments, attachmentsLinks);
    }

    public async Task<(List<Attachment> Attachments, List<string> AttachmentsLinks)> ProcessAttachments
    (IEnumerable<AttachmentModelRequest>? attachmentRequests, Domain.Entities.PracticalLessonItemSubmit entity, string attachmentType)
    {
        var attachments = new List<Attachment>();
        var attachmentsLinks = new List<string>();

        if (attachmentRequests == null)
            return (attachments, attachmentsLinks);

        foreach (var attachmentRequest in attachmentRequests)
        {
            var newFileName = $"{Guid.NewGuid()}_{attachmentType}_{attachmentRequest.Name}";

            var uploadingResult = await _filesManager.UploadFile(attachmentRequest.Stream, newFileName, null);
            if (uploadingResult.IsRight)
                continue;

            var fileSuccess = (FileSuccess)uploadingResult;

            var attachment = new Attachment { Url = fileSuccess.Name, PracticalLessonItemSubmit = entity };
            attachments.Add(attachment);

            attachmentsLinks.Add(fileSuccess.Url);
        }

        return (attachments, attachmentsLinks);
    }
}
