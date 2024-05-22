using System.Net.Mail;

namespace Mezl.Result.Validation;

public static class PropertyValidatorExtensions
{
    public static PropertyValidator<T> NotNull<T>(this PropertyValidator<T> context, string messageOnError = "Is Null")
    {
        if (context.Value == null)
        {
            context.Add(messageOnError);
        }

        return context;
    }

    public static PropertyValidator<T> InRange<T>(this PropertyValidator<T> context, T from, T to, string messageOnError = "Is Not in Range <{0}, {1}>") where T : IComparable
    {
        if (context.Value.CompareTo(from) < 0 ||  context.Value.CompareTo(to) > 0)
        {
            context.Add(string.Format(messageOnError, from, to));
        }

        return context;
    }

    public static PropertyValidator<string> IsEmail(this PropertyValidator<string> context, string messageOnError = "Is Not Valid Email")
    {
        if (MailAddress.TryCreate(context.Value, out _) == false)
        {
            context.Add(messageOnError);
        }

        return context;
    }

    public static PropertyValidator<string> MaxLength(this PropertyValidator<string> context, int maxLength, string messageOnError = "Is Longer Than {0}")
    {
        if (context.Value.Length > maxLength)
        {
            context.Add(string.Format(messageOnError, maxLength));
        }

        return context;
    }

    public static PropertyValidator<string> MinLength(this PropertyValidator<string> context, int minLength, string messageOnError = "Is Shorter Than {0}")
    {
        if (context.Value.Length < minLength)
        {
            context.Add(string.Format(messageOnError, minLength));
        }

        return context;
    }

    public static PropertyValidator<T> Should<T>(this PropertyValidator<T> context, Func<T, bool> validator, string messageOnError = "Is Not Valid")
    {
        if (validator(context.Value) == false)
        {
            context.Add(messageOnError);
        }

        return context;
    }

    public static async Task<PropertyValidator<T>> ShouldAsync<T>(this PropertyValidator<T> context, Func<T, Task<bool>> validator, string messageOnError = "Is Not Valid")
    {
        if (await validator(context.Value) == false)
        {
            context.Add(messageOnError);
        }

        return context;
    }
}