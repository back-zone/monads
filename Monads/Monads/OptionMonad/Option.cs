using System;
using System.Threading.Tasks;

namespace Back.Zone.Monads.OptionMonad;

public abstract class Option<TSomeTypeA>
{
    public abstract bool IsEmpty();

    public abstract TSomeTypeA Get();

    public bool IsDefined() => !IsEmpty();

    public Option<TSomeTypeB> Map<TSomeTypeB>(Func<TSomeTypeA, TSomeTypeB> func) =>
        IsEmpty() ? new None<TSomeTypeB>() : new Some<TSomeTypeB>(func(Get()));

    public async Task<Option<TSomeTypeB>> MapAsync<TSomeTypeB>(Func<TSomeTypeA, Task<TSomeTypeB>> func) =>
        IsEmpty() ? new None<TSomeTypeB>() : new Some<TSomeTypeB>(await func(Get()));

    public Option<TSomeTypeB> Flatmap<TSomeTypeB>(Func<TSomeTypeA, Option<TSomeTypeB>> func) =>
        IsEmpty() ? new None<TSomeTypeB>() : func(Get());

    public async Task<Option<TSomeTypeB>> FlatmapAsync<TSomeTypeB>(
        Func<TSomeTypeA, Task<Option<TSomeTypeB>>> func
    ) => IsEmpty() ? new None<TSomeTypeB>() : await func(Get());

    public TSomeTypeB Fold<TSomeTypeB>(TSomeTypeB ifEmptyValue, Func<TSomeTypeA, TSomeTypeB> func) =>
        IsEmpty() ? ifEmptyValue : func(Get());

    public async Task<TSomeTypeB> FoldAsync<TSomeTypeB>(TSomeTypeB ifEmptyValue,
        Func<TSomeTypeA, Task<TSomeTypeB>> func) =>
        IsEmpty() ? ifEmptyValue : await func(Get());

    public async Task<TSomeTypeB> FoldAsync<TSomeTypeB>(Func<TSomeTypeB> ifEmpty,
        Func<TSomeTypeA, Task<TSomeTypeB>> func) =>
        IsEmpty() ? ifEmpty() : await func(Get());

    public TSomeTypeA GetOrElse(TSomeTypeA elseVal) => IsEmpty() ? elseVal : Get();
}