using System;

namespace Back.Zone.Monads.OptionMonad;

public sealed class None<TA> : Option<TA>
{
    public override bool IsEmpty() => true;

    public override TA Get()
    {
        throw new NullReferenceException("Get() Called on None!");
    }
}