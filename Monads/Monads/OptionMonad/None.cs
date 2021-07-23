using System;

namespace Back.Zone.Monads.OptionMonad
{
    public sealed class None<TA> : Option<TA>
    {
        public override TA Get()
        {
            throw new NullReferenceException("None.Get");
        }

        public override bool IsEmpty() => true;
    }
}