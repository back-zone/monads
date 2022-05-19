using System;
using System.Threading.Tasks;
using Back.Zone.Monads.TryMonad;

namespace Back.Zone.Monads.OptionMonad;

public static class Option
{
    public static Option<TSomeTypeA> ToOption<TSomeTypeA>(this TSomeTypeA value) =>
        value == null ? new None<TSomeTypeA>() : new Some<TSomeTypeA>(value);
    
    public static Option<TSomeTypeA> From<TSomeTypeA>(TSomeTypeA value) =>
        value is not null ? new Some<TSomeTypeA>(value) : new None<TSomeTypeA>();

    public static async Task<Option<TSomeTypeA>> FromAsync<TSomeTypeA>(Task<TSomeTypeA> valueTask) =>
        await Try.FromAsync(valueTask).ToOptionAsync();

    public static Option<TSomeTypeB> Map<TSomeTypeA, TSomeTypeB>(
        this Option<TSomeTypeA> option,
        Func<TSomeTypeA, TSomeTypeB> func
    ) => option.IsEmpty() ? new None<TSomeTypeB>() : new Some<TSomeTypeB>(func(option.Get()));

    public static async Task<Option<TSomeTypeB>> MapAsync<TSomeTypeA, TSomeTypeB>(
        this Task<Option<TSomeTypeA>> optionTask,
        Func<TSomeTypeA, Task<TSomeTypeB>> func
    )
    {
        var option = await optionTask;
        return option.IsEmpty() ? new None<TSomeTypeB>() : new Some<TSomeTypeB>(await func(option.Get()));
    }

    public static Option<TSomeTypeB> Flatmap<TSomeTypeA, TSomeTypeB>(
        this Option<TSomeTypeA> option,
        Func<TSomeTypeA, Option<TSomeTypeB>> func
    ) => option.IsEmpty() ? new None<TSomeTypeB>() : func(option.Get());

    public static async Task<Option<TSomeTypeB>> FlatmapAsync<TSomeTypeA, TSomeTypeB>(
        this Task<Option<TSomeTypeA>> optionTask,
        Func<TSomeTypeA, Task<Option<TSomeTypeB>>> func
    )
    {
        var option = await optionTask;
        return option.IsEmpty() ? new None<TSomeTypeB>() : await func(option.Get());
    }

    public static TSomeTypeB Fold<TSomeTypeA, TSomeTypeB>(
        this Option<TSomeTypeA> option,
        TSomeTypeB ifEmptyValue,
        Func<TSomeTypeA, TSomeTypeB> func
    ) => option.IsEmpty() ? ifEmptyValue : func(option.Get());
    
    public static async Task<TSomeTypeB> FoldAsync<TSomeTypeA, TSomeTypeB>(
        this Task<Option<TSomeTypeA>> optionTask,
        TSomeTypeB ifEmptyValue,
        Func<TSomeTypeA, Task<TSomeTypeB>> func
    )
    {
        var option = await optionTask;
        return option.IsEmpty() ? ifEmptyValue : await func(option.Get());
    }

    public static async Task<TSomeTypeB> FoldAsync<TSomeTypeA, TSomeTypeB>(
        this Task<Option<TSomeTypeA>> optionTask,
        Func<TSomeTypeB> ifEmpty,
        Func<TSomeTypeA, Task<TSomeTypeB>> func
    )
    {
        var option = await optionTask;
        return option.IsEmpty() ? ifEmpty() : await func(option.Get());
    }
    
    public static TUnifiedType Fold<TUnifiedType, TSomeTypeA>(
        this Option<TSomeTypeA> option,
        Func<TUnifiedType> none,
        Func<TSomeTypeA, TUnifiedType> some) => option switch
    {
        None<TSomeTypeA> _ => none(),
        Some<TSomeTypeA>(var value) => some(value),
        _ => none()
    };
    
    public static async Task<TUnifiedType> FoldAsync<TUnifiedType, T>(
        this Task<Option<T>> option,
        Func<TUnifiedType> none,
        Func<T, Task<TUnifiedType>> some) => await option switch
    {
        None<T> _ => none(),
        Some<T>(var value) => await some(value),
        _ => none()
    };

    public static async Task<TUnifiedType> FoldAsync<TUnifiedType, T>(
        this Task<Option<T>> option,
        Func<Task<TUnifiedType>> none,
        Func<T, TUnifiedType> some) => await option switch
    {
        None<T> _ => await none(),
        Some<T>(var value) => some(value),
        _ => await none()
    };

    public static async Task<TUnifiedType> FoldAsync<TUnifiedType, T>(
        this Task<Option<T>> option,
        Func<Task<TUnifiedType>> none,
        Func<T, Task<TUnifiedType>> some) => await option switch
    {
        None<T> _ => await none(),
        Some<T>(var value) => await some(value),
        _ => await none()
    };
}