namespace FindYourFriendAmongPets.Core.Shared;

public static class Errors
{
    public static class General
    {
        public const string VALIDATION_ERROR_CODE = "value.is.invalid";
        
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

    public static class Model
    {
        public static Error AlreadyExist(string? name = null)
        {
            var label = name ?? "entity";
            return Error.Validation($"{label}.already.exist", $"{label} already exist");
        }
    }
}