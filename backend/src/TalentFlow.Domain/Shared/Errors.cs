﻿namespace TalentFlow.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error AlreadyExist(string? name = null)
        {
            var label = name ?? "entity";
            return Error.Validation($"{label}_already_exist", $"{label} already exist");
        }

        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value_is_invalid", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id - '{id}'";
            return Error.NotFound("record_not_found", $"record not found {forId}");
        }

        public static Error ValueIsRequired(string? name = null, int length = 0)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("length_is_invalid", $"invalid {label} length - {length}");
        }

        public static Error InsufficientItems(string? name = null)
        {
            var label = name ?? "items";
            return Error.Validation("insufficient_items", $"Insufficient number of {label} to complete the operation");
        }

        public static Error Null(string? name = null)
        {
            var label = name ?? "value";
            return Error.Null("Null_entity", $"{label} is null");
        }
    }
}