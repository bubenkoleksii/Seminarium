namespace SchoolManagementService.Core.Application.Common.CloudStorage.Models;

public record CloudStorageItem(
    string FileName,
    string Folder,
    Stream Stream);
