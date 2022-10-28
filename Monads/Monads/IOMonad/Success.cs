using System;

namespace Back.Zone.Monads.IOMonad;

public sealed class Success<TA> : IO<TA>
{
    private readonly TA _value;

    public Success(TA value)
    {
        _value = value;
    }

    public override bool IsSuccess() => true;

    public override TA Get() => _value;

    public override Exception Error() => throw new Exception("Error() called on Success");

    public static Success<TA> From(TA value) => new(value);

    public static implicit operator Success<TA>(TA value) => new(value);

    public void Deconstruct(out TA value) => value = _value;
}

public static class Success
{
    public static Success<TA> From<TA>(TA value) => new(value);
}