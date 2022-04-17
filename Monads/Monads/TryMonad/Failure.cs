using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.TryMonad;

public sealed class Failure<TTryTypeA> : Try<TTryTypeA>
{
    private readonly Exception _exception;

    public Failure(Exception exception)
    {
        _exception = exception;
    }

    public override bool IsFailure() => true;

    public override bool IsSuccess() => false;

    public override TTryTypeA Get() => throw _exception;

    public override Try<TTryTypeB> Map<TTryTypeB>(
        Func<TTryTypeA, TTryTypeB> func
    ) => throw _exception;

    public override Task<Try<TTryTypeB>> MapAsync<TTryTypeB>(
        Func<TTryTypeA, Task<TTryTypeB>> func
    ) => throw _exception;

    public override Try<TTryTypeB> FlatMap<TTryTypeB>(
        Func<TTryTypeA, Try<TTryTypeB>> func
    ) => throw _exception;

    public override Task<Try<TTryTypeB>> FlatmapAsync<TTryTypeB>(
        Func<TTryTypeA, Task<Try<TTryTypeB>>> func
    ) => throw _exception;

    public override TUnifiedType Fold<TUnifiedType>(
        Func<Exception, TUnifiedType> exceptionFunc,
        Func<TTryTypeA, TUnifiedType> func
    ) => exceptionFunc(_exception);

    public override async Task<TUnifiedType> FoldAsync<TUnifiedType>(
        Func<Exception, TUnifiedType> exceptionFunc,
        Func<TTryTypeA, Task<TUnifiedType>> func
    ) => await Task.FromResult(exceptionFunc(_exception));

    public void Deconstruct(out Exception exception) => exception = _exception;
}