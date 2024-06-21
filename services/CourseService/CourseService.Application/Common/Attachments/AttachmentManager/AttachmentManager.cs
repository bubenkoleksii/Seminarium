namespace CourseService.Application.Common.Attachments.AttachmentManager;

public class AttachmentManager(IFilesManager filesManager, ICommandContext commandContext) : IAttachmentManager
{
    private readonly IFilesManager _filesManager = filesManager;

    private readonly ICommandContext _commandContext = commandContext;

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

    public async Task DeleteAttachments(IEnumerable<Attachment>? attachments, CancellationToken cancellationToken)
    {
        if (attachments == null)
            return;

        foreach (var attachment in attachments)
        {
            await _filesManager.DeleteFileIfExists(attachment.Url);

            _commandContext.Attachments.Remove(attachment);
        }

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while deleting attachments");
        }
    }
}
