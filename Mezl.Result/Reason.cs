using System.Runtime.CompilerServices;

namespace Mezl.Result;

public abstract class Reason
{
    public string Message { get; private init; }
    public string MethodInfo { get; private init; }

    private List<string> _additionalInfo;
    public IReadOnlyCollection<string> AdditionalInfo => _additionalInfo;

    protected Reason() { }

    public Reason AddInfo(string info)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));

        _additionalInfo ??= new List<string>();
        _additionalInfo.Add(info);
        return this;
    }

    public static TReason New<TReason>(string message = null, [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = -1, [CallerFilePath] string filePath = "") where TReason : Reason, new()
    {
        return new TReason { Message = message, MethodInfo = $" - {filePath} -> {methodName} -> {lineNumber}" };
    }
}