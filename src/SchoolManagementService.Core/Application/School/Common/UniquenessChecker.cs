using SchoolManagementService.Core.Application.Common.DataContext;
using SchoolManagementService.Core.Domain.Errors;
using SchoolManagementService.Core.Domain.Errors.AlreadyExists;
using SchoolManagementService.Core.Domain.Errors.AlreadyExists.School;

namespace SchoolManagementService.Core.Application.School.Common;

internal static class UniquenessChecker
{
    public static bool GetErrorIfAlreadyExists(ICommandContext commandContext, Domain.School entity, out Error? error)
    {
        var existedEntity = commandContext.Schools
            .AsNoTracking()
            .FirstOrDefault(s => s.RegisterCode == entity.RegisterCode ||
                                 (entity.Email != null && s.Email == entity.Email) ||
                                 (entity.Phone != null && s.Phone == entity.Phone));

        if (existedEntity == null)
        {
            error = null;
            return false;
        }

        if (existedEntity.RegisterCode == entity.RegisterCode)
        {
            error = new RegisterCodeAlreadyExists { Params = { nameof(entity.RegisterCode).ToLower() } };
            return true;
        }

        if (existedEntity.Email == entity.Email)
        {
            error = new EmailAlreadyExistsError
            {
                Detail = "The school with this email address already exists.",
                Params = { nameof(entity.Email).ToLower() }
            };
            return true;
        }

        if (existedEntity.Phone == entity.Phone)
        {
            error = new PhoneAlreadyExistsError
            {
                Detail = "The school with this phone already exists.",
                Params = { nameof(entity.Phone).ToLower() }
            };
            return true;
        }

        error = new AlreadyExistsError();
        return true;
    }
}
