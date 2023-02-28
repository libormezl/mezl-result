using System.Runtime.CompilerServices;

namespace Mezl.Result;


public abstract class Reason
{
    public string? Message { get; private set; }

    private List<string>? _additionalInfo;
    public IReadOnlyCollection<string>? AdditionalInfo => _additionalInfo;

    public string MethodInfo { get; private init; }

    public Reason WithMessage(string message)
    {
        if (Message != null)
        {
            throw new InvalidOperationException("Message is already set");
        }

        Message = message;
        return this;
    }

    public Reason AddInfo(string info)
    {
        _additionalInfo ??= new List<string>();
        _additionalInfo.Add(info);
        return this;
    }

    public static TReason New<TReason>([CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1, [CallerFilePath] string filePath = "") where TReason : Reason, new()
    {
        return new TReason() { MethodInfo = $" - {filePath} -> {methodName} -> {lineNumber}" };
    }
}

public class ReasonAlreadyExists : Reason { }
public class ReasonInternalError : Reason { }
public class ReasonNotFound : Reason { }
public class ReasonUnauthorized : Reason { }
public class ReasonInvalidOperation : Reason { }
