using System;

namespace Back.Zone.Monads.EitherMonad;

public sealed class Right<TE, TA> : Either<TE, TA>
{
    private readonly TA _value;

    public Right(TA value)
    {
        _value = value;
    }

    public override bool IsLeft() => false;

    public override TE LeftValue()
    {
        throw new Exception("Left() called on Right()");
    }

    public override TA RightValue() => _value;

    public static implicit operator Right<TE, TA>(TA value) => new(value);

    public void Deconstruct(out TA value) => value = _value;
}

public static class Right
{
    public static Right<TE, TA> From<TE, TA>(TA value) => new(value);
}