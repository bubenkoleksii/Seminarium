using SchoolManagementService.Core.Application.Common.CloudStorage;
using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Files;

[AttributeUsage(AttributeTargets.Parameter)]
public class FileValidationAttribute(FileKind kind, int maxAllowedSizeInMb = 1024) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var isFormFileType = IsFormFileType(value.GetType());
        var isEnumerableFormFileType = IsEnumerableFormFileType(value.GetType());

        if (!isFormFileType && !isEnumerableFormFileType)
            return ValidationResult.Success;

        validationContext.GetService(typeof(IModelMetadataProvider));
        if (validationContext.GetService(typeof(ModelStateDictionary)) is not ModelStateDictionary modelState)
            return ValidationResult.Success;

        if (isFormFileType)
        {
            var formFile = (value as FormFile)!;
            return ValidateFile(formFile, modelState, kind, maxAllowedSizeInMb);
        }

        var formFiles = (value as IEnumerable<FormFile>)!;
        return ValidateFiles(formFiles, modelState, kind, maxAllowedSizeInMb);
    }

    private static ValidationResult? ValidateFiles(IEnumerable<FormFile> formFiles, ModelStateDictionary modelState, FileKind kind, int maxAllowedSizePerItemInMb)
    {
        foreach (var formFile in formFiles)
        {
            var result = ValidateFile(formFile, modelState, kind, maxAllowedSizePerItemInMb);
            if (modelState.ContainsKey(formFile.Name))
                return result;
        }

        return ValidationResult.Success;
    }

    private static ValidationResult? ValidateFile(IFormFile formFile, ModelStateDictionary modelState, FileKind kind, int maxAllowedSizeInMb)
    {
        using var fileStream = formFile.OpenReadStream();

        var isRecognizableType = FileTypeValidator.IsTypeRecognizable(fileStream);
        var isInvalidImageType = kind == FileKind.Image && !fileStream.IsImage();

        if (!isRecognizableType || isInvalidImageType)
        {
            modelState.AddModelError(formFile.Name, ErrorTitles.File.NotRecognizable);
            return ValidationResult.Success;
        }

        var sizeInMb = formFile.Length / (1024.0 * 1024);
        if (sizeInMb > maxAllowedSizeInMb)
        {
            modelState.AddModelError(formFile.Name, ErrorTitles.File.LargerMaxSize);
        }

        return ValidationResult.Success;
    }

    private static bool IsFormFileType(Type type) => type == typeof(FormFile);

    private static bool IsEnumerableFormFileType(Type type) =>
        type.IsGenericType &&
        type.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
        type.GetGenericArguments().Exists(arg => arg == typeof(FormFile));

}
