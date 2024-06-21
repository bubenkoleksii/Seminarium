namespace CourseService.Application.Common.Attachments.AttachmentManager;

public interface IAttachmentManager
{
    public Task<(List<Attachment> Attachments, List<string> AttachmentsLinks)> ProcessAttachments(
      IEnumerable<AttachmentModelRequest>? attachmentRequests, Domain.Entities.LessonItem entity, string attachmentType);

    public Task<(List<Attachment> Attachments, List<string> AttachmentsLinks)> ProcessAttachments(
        IEnumerable<AttachmentModelRequest>? attachmentRequests, Domain.Entities.PracticalLessonItemSubmit entity, string attachmentType);

    public Task DeleteAttachments(IEnumerable<Attachment>? attachments, CancellationToken cancellationToken);
}
