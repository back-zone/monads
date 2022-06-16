using System;
using System.Threading.Tasks;
using Back.Zone.Monads.TryMonad;
using Back.Zone.Monads.Validated;

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
    ) => option.Map(func);

    public static async Task<Option<TSomeTypeB>> MapAsync<TSomeTypeA, TSomeTypeB>(
        this Task<Option<TSomeTypeA>> optionTask,
        Func<TSomeTypeA, Task<TSomeTypeB>> func
    )
    {
        var option = await optionTask;
        return await option.MapAsync(func);
    }

    public static Option<TSomeTypeB> Flatmap<TSomeTypeA, TSomeTypeB>(
        this Option<TSomeTypeA> option,
        Func<TSomeTypeA, Option<TSomeTypeB>> func
    ) => option.Flatmap(func);

    public static async Task<Option<TSomeTypeB>> FlatmapAsync<TSomeTypeA, TSomeTypeB>(
        this Task<Option<TSomeTypeA>> optionTask,
        Func<TSomeTypeA, Task<Option<TSomeTypeB>>> func
    )
    {
        var option = await optionTask;
        return await option.FlatmapAsync(func);
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
        None<TSomeTypeA> => none(),
        Some<TSomeTypeA>(var value) => some(value),
        _ => none()
    };
    
    public static async Task<TUnifiedType> FoldAsync<TUnifiedType, T>(
        this Task<Option<T>> option,
        Func<TUnifiedType> none,
        Func<T, Task<TUnifiedType>> some) => await option switch
    {
        None<T> => none(),
        Some<T>(var value) => await some(value),
        _ => none()
    };

    public static async Task<TUnifiedType> FoldAsync<TUnifiedType, T>(
        this Task<Option<T>> option,
        Func<Task<TUnifiedType>> none,
        Func<T, TUnifiedType> some) => await option switch
    {
        None<T> => await none(),
        Some<T>(var value) => some(value),
        _ => await none()
    };

    public static async Task<TUnifiedType> FoldAsync<TUnifiedType, T>(
        this Task<Option<T>> option,
        Func<Task<TUnifiedType>> none,
        Func<T, Task<TUnifiedType>> some) => await option switch
    {
        None<T> => await none(),
        Some<T>(var value) => await some(value),
        _ => await none()
    };

    public static Validated<Exception, TASomeType> ToValidated<TASomeType>(
        this Option<TASomeType> option)
        => option switch
        {
            None<TASomeType> => new ValidatedFailure<Exception, TASomeType>(new Exception($"{nameof(option)} is empty!")),
            Some<TASomeType>(var value) => new ValidatedSuccess<Exception, TASomeType>(value),
            _ => new ValidatedFailure<Exception, TASomeType>(new Exception($"{nameof(option)} was malformed!"))
        };

    public static async Task<Validated<Exception, TASomeType>> ToValidatedAsync<TASomeType>(
        this Task<Option<TASomeType>> optionTask
    ) => (await optionTask).ToValidated();
}