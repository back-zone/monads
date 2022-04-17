using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.TryMonad;

public abstract class Try<TTryTypeA>
{
    public abstract bool IsFailure();

    public abstract bool IsSuccess();

    public abstract TTryTypeA Get();

    public abstract Try<TTryTypeB> Map<TTryTypeB>(Func<TTryTypeA, TTryTypeB> func);

    public abstract Task<Try<TTryTypeB>> MapAsync<TTryTypeB>(Func<TTryTypeA, Task<TTryTypeB>> func);

    public abstract Try<TTryTypeB> FlatMap<TTryTypeB>(Func<TTryTypeA, Try<TTryTypeB>> func);

    public abstract Task<Try<TTryTypeB>> FlatmapAsync<TTryTypeB>(Func<TTryTypeA, Task<Try<TTryTypeB>>> func);

    public abstract TUnifiedType Fold<TUnifiedType>(
        Func<Exception, TUnifiedType> exceptionFunc,
        Func<TTryTypeA, TUnifiedType> func
    );

    public abstract Task<TUnifiedType> FoldAsync<TUnifiedType>(
        Func<Exception, TUnifiedType> exceptionFunc,
        Func<TTryTypeA, Task<TUnifiedType>> func
    );
}