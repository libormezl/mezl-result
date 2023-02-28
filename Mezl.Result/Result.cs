namespace Mezl.Result;

public readonly struct Result
{
    public static Result Success = new();

    private Result(Reason value)
    {
        Reason = value;
    }

    public readonly Reason Reason;

    public bool IsSuccessful => Reason is null;
    public bool IsNotSuccessful => Reason is not null;

    public bool Is<T>() where T : Reason
    {
        return IsSuccessful && Reason is T;
    }

    public static implicit operator Result(Reason reason) => new(reason);

    public static implicit operator Reason(Result value) => value.Reason;
}

public readonly struct Result<TValue>
{
    private Result(TValue value)
    {
        Value = value;
        Reason = null;
    }

    private Result(Reason value)
    {
        Value = default;
        Reason = value;
    }

    public readonly TValue Value;

    public readonly Reason Reason;

    public bool IsSuccessful => Reason is null;
    public bool IsNotSuccessful => Reason is not null;

    public bool Is<T>() where T : Reason
    {
        return IsSuccessful && Reason is T;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(Reason reason) => new(reason);

    public static implicit operator TValue(Result<TValue> value) => value.Value;

    public static implicit operator Reason(Result<TValue> value) => value.Reason;
}