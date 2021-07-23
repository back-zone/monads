namespace Back.Zone.Monads.OptionMonad
{
    public sealed class Some<TA> : Option<TA>
    {
        private readonly TA _value;

        public Some(TA value)
        {
            _value = value;
        }

        public override TA Get() => _value;

        public override bool IsEmpty() => false;

        public void Deconstruct(out TA value) => value = _value;
    }
}