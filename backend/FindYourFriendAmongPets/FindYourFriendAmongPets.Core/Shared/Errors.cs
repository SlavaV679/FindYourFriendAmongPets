﻿namespace FindYourFriendAmongPets.Core.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null, string? messageContent = null)
        {
            var label = name ?? "value";

            var message = messageContent == null ? string.Empty : $": {messageContent}";
            
            return Error.Validation("value.is.invalid", $"{label} is invalid{message}");
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

    public static class Volunteer
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "Volunteer already exist");
        }
    }
}