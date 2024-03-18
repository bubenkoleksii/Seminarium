namespace SchoolService.Application.School.Common;

internal static class UniquenessChecker
{
    public static bool GetErrorIfAlreadyExists(ICommandContext commandContext, Domain.Entities.School entity,
        out Error? error)
    {
        var existedEntity = commandContext.Schools
            .AsNoTracking()
            .FirstOrDefault(s => s.RegisterCode == entity.RegisterCode ||
                                 entity.Email != null && string.CompareOrdinal(s.Email, entity.Email) == 0 ||
                                 entity.Phone != null && s.Phone == entity.Phone);

        if (existedEntity is null)
        {
            error = null;
            return false;
        }

        if (existedEntity.RegisterCode == entity.RegisterCode)
        {
            error = new RegisterCodeAlreadyExists(entity.RegisterCode);
            return true;
        }

        if (existedEntity.Email == entity.Email)
        {
            error = new EmailAlreadyExistsError(entity.Email!, "school");
            return true;
        }

        if (existedEntity.Phone == entity.Phone)
        {
            error = new PhoneAlreadyExistsError(entity.Phone!, "school");
            return true;
        }

        error = new AlreadyExistsError("school");
        return true;
    }
}
