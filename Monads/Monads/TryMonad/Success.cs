using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.TryMonad;

public sealed class Success<TTryTypeA> : Try<TTryTypeA>
{
    private readonly TTryTypeA _value;

    public Success(TTryTypeA value)
    {
        _value = value;
    }

    public override bool IsFailure() => false;

    public override bool IsSuccess() => true;

    public override TTryTypeA Get() => _value;

    public override Try<TTryTypeB> Map<TTryTypeB>(
        Func<TTryTypeA, TTryTypeB> func
    ) => Try.From(func(_value));

    public override async Task<Try<TTryTypeB>> MapAsync<TTryTypeB>(
        Func<TTryTypeA, Task<TTryTypeB>> func
    ) => await Try.FromAsync(func(_value));

    public override Try<TTryTypeB> Flatmap<TTryTypeB>(
        Func<TTryTypeA, Try<TTryTypeB>> func
    ) => func(_value);

    public override async Task<Try<TTryTypeB>> FlatmapAsync<TTryTypeB>(
        Func<TTryTypeA, Task<Try<TTryTypeB>>> func
    ) => await func(_value);

    public override TUnifiedType Fold<TUnifiedType>(
        Func<Exception, TUnifiedType> exceptionFunc,
        Func<TTryTypeA, TUnifiedType> func
    ) => func(_value);

    public override async Task<TUnifiedType> FoldAsync<TUnifiedType>(
        Func<Exception, TUnifiedType> exceptionFunc,
        Func<TTryTypeA, Task<TUnifiedType>> func
    ) => await func(_value);

    public void Deconstruct(out TTryTypeA value) => value = _value;
}