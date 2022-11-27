namespace Back.Zone.Monads.OptionMonad;

public sealed class Some<TA>: Option<TA>
{
    private readonly TA _value;

    public Some(TA value)
    {
        _value = value;
    }

    public override bool IsEmpty() => false;

    public override TA Get() => _value;

    public void Deconstruct(out TA value) => value = _value;
}