namespace Mezl.Result;


public class R
{
    public static R Success = new();

    protected R()
    {
        Reason = null;
    }

    protected R(Reason value)
    {
        Reason = value;
    }

    public readonly Reason Reason;
    public bool IsSuccessful => Reason is null;
    public bool IsNotSuccessful => Reason is not null;

    public static implicit operator R(Reason reason) => new(reason);

    public static implicit operator Reason(R value) => value.Reason;
}

public class R<TValue> : R
{
    protected internal R(TValue value)
    {
        Value = value;
    }

    protected internal R(Reason value) : base(value)
    {
        Value = default;
    }

    public readonly TValue Value;

    public static implicit operator R<TValue>(TValue value) => new(value);

    public static implicit operator R<TValue>(Reason reason) => new(reason);

    public static implicit operator TValue(R<TValue> value) => value.Value;

    public static implicit operator Reason(R<TValue> value) => value.Reason;
}

/// <summary>
/// For these who do not like R
/// </summary>
public class Result<TValue> : R<TValue>
{
    protected internal Result(TValue value) : base(value) { }

    protected internal Result(Reason value) : base(value) { }
}

/// <summary>
/// For these who do not like R
/// </summary>
public class Result : R
{
    protected internal Result(Reason value) : base(value) { }
}