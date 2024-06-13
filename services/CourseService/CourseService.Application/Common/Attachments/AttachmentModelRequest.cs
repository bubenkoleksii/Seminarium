namespace CourseService.Application.Common.Attachments;

public class AttachmentModelRequest
{
    public required string Name { get; set; }

    public required Stream Stream { get; set; }
}
