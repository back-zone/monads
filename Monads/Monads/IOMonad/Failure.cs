using System;

namespace Back.Zone.Monads.IOMonad;

public sealed class Failure<TA> : IO<TA>
{
    private readonly Exception _value;

    public Failure(Exception value)
    {
        _value = value;
    }

    public override bool IsSuccess() => false;

    public override TA Get() => throw new Exception("Get() called on Failure");

    public override Exception Error() => _value;

    public static Failure<TA> From(Exception exception) => new(exception);

    public static implicit operator Failure<TA>(Exception exception) => new(exception);

    public void Deconstruct(out Exception exception) => exception = _value;
}

public static class Failure
{
    public static Failure<TA> From<TA>(Exception exception) => new(exception);
}