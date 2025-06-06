﻿using TalentFlow.Domain.Shared;

namespace TalentFlow.API.Response;

public record Envelope
{
    public object? Result { get; set;}
    public ErrorList? Errors { get; init; }
    public DateTime TimeCreated { get; }

    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        TimeCreated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Error(ErrorList errors) =>
        new(null, errors);
}
