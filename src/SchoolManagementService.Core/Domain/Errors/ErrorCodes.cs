namespace SchoolManagementService.Core.Domain.Errors;

public static class ErrorCodes
{
    public static class Common
    {
        public static readonly string EmailAlreadyExists = "email_already_exists";

        public static readonly string PhoneAlreadyExists = "phone_already_exists";

        public static readonly string NotFoundById = "not_found_by_id";

        public static readonly string Unknown = "unknown";
    }

    public static class School
    {
        public static readonly string RegisterCodeAlreadyExists = "register_code_already_exists";
    }
}
