namespace PetFriend.SharedKernel;

public static class Errors
{
    public static class General
    {
        public const string VALIDATION_ERROR_CODE = "value.is.invalid";
        
        public static Error AlreadyExist(string? name = null)
        {
            var label = name ?? "entity";
            return Error.Validation($"{label}.already.exist", $"{label} already exist");
        }
        
        public static Error ValueIsInvalid(string? propertyName = null, string? detailedMessage = null)
        {
            var label = propertyName ?? "value";

            var message = detailedMessage == null ? string.Empty : $": {detailedMessage}";
            
            return Error.Validation(VALIDATION_ERROR_CODE, $"{label} is invalid{message}");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : $"{name} ";
            return Error.Validation("length.is.invalid", $"invalid {label}length)");
        }
    }
    
    public static class User
    {
        public static Error InvalidCredentials()
        {
            return Error.Validation("invalid.user.credentials", "Invalid user credentials");
        }
    }
    
    public static class Token
    {
        public static Error ExpiredToken()
        {
            return Error.Validation("token.is.expired", "Your token is expired. Please, login again");
        }

        public static Error InvalidToken()
        {
            return Error.Validation("token.is.invalid", "Your token is invalid. Please, login again");
        }
    }
}