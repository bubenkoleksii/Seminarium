using SchoolManagementService.Core.Application.Common.CloudStorage.Models;

namespace SchoolManagementService.Core.Application.Common.CloudStorage;

public interface ICloudStorageManager
{
    public Task Upload(CloudStorageItem item);
}
