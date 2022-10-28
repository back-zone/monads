using System;

namespace Back.Zone.Monads.EitherMonad;

public sealed class Left<TE, TA> : Either<TE, TA>
{
    private readonly TE _value;

    public Left(TE value)
    {
        _value = value;
    }

    public override bool IsLeft() => true;

    public override TE LeftValue() => _value;

    public override TA RightValue()
    {
        throw new Exception("Right() called on Left()");
    }

    public static implicit operator Left<TE, TA>(TE value) => new(value);

    public void Deconstruct(out TE value) => value = _value;
}

public static class Left
{
    public static Left<TE, TA> From<TE, TA>(TE value) => new(value);
}