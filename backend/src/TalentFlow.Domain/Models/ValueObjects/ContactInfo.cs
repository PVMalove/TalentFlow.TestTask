using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.Models.ValueObjects;

public record ContactInfo
{
    private const string PhoneRegex = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\-]?)?[\d\-]{7,10}$";

    public string Email { get; }
    public string Phone { get; }

    private ContactInfo(string email, string phone)
    {
        Email = email;
        Phone = phone;
    }

    public static Result<ContactInfo, Error> Create(string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Errors.General.ValueIsInvalid("Email");

        if (string.IsNullOrWhiteSpace(phone) || Regex.IsMatch(phone, PhoneRegex) == false)
            return Errors.General.ValueIsInvalid("Phone");

        return new ContactInfo(email, phone);
    }
}