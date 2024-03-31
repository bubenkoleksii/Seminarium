namespace Shared.Contracts.Errors
{
    public static class ErrorTitles
    {
        public static class Common
        {
            public static readonly string Empty = "empty";

            public static readonly string Null = "null";

            public static readonly string TooLong = "too_long";

            public static readonly string LowerZero = "lower_zero";

            public static readonly string Invalid = "invalid";

            public static readonly string InvalidSize = "invalid_size";

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
}
