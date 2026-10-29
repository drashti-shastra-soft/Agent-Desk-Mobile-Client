using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgentDesk.Helpers;

public static class ValidationHelper
{
    //public static ValidationResult? ValidateEmail(
    //    string? email,
    //    ValidationContext context)
    //{
    //    if (string.IsNullOrWhiteSpace(email))
    //        return ValidationResult.Success;

    //    email = email.Trim();

    //    try
    //    {
    //        var address = new MailAddress(email);

    //        bool isValid =
    //            address.Address == email &&
    //            address.Host.Contains('.') &&
    //            !address.Host.EndsWith('.');

    //        return isValid
    //            ? ValidationResult.Success
    //            : new ValidationResult(
    //                "Please enter a valid email address.");
    //    }
    //    catch (FormatException)
    //    {
    //        return new ValidationResult(
    //            "Please enter a valid email address.");
    //    }
    //}

    public static ValidationResult? ValidateEmail(
    string? email,
    ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(email))
            return ValidationResult.Success;

        email = email.Trim();

        const string pattern =
            @"^(?!.*\.\.)[A-Za-z0-9_%+-]+(?:\.[A-Za-z0-9_%+-]+)*@" +
            @"(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+" +
            @"[A-Za-z]{2,}$";

        if (!Regex.IsMatch(email, pattern))
        {
            return new ValidationResult(
                "Please enter a valid email address.");
        }

        try
        {
            var address = new MailAddress(email);

            bool isValid =
                address.Address == email &&
                address.Host.Contains('.') &&
                !address.Host.EndsWith('.');

            return isValid
                ? ValidationResult.Success
                : new ValidationResult(
                    "Please enter a valid email address.");
        }
        catch (FormatException)
        {
            return new ValidationResult(
                "Please enter a valid email address.");
        }
    }

    public static ValidationResult? ValidateMobileNumber(
        string? mobileNumber,
        ValidationContext context)
    {
        // Required attribute handles empty values.
        if (string.IsNullOrWhiteSpace(mobileNumber))
            return ValidationResult.Success;

        // Keep only digits, allowing formatted values such as "78457 84578".
        var mobileDigits = new string(
            mobileNumber.Where(char.IsDigit).ToArray());

        // Validate length.
        if (mobileDigits.Length != 10)
        {
            return new ValidationResult(
                "Please enter a valid 10-digit mobile number.");
        }

        // Mobile number must start with 6, 7, 8, or 9.
        if (!"6789".Contains(mobileDigits[0]))
        {
            return new ValidationResult(
                "Mobile number must start with 6, 7, 8, or 9.");
        }

        return ValidationResult.Success;
    }
}

