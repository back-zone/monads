using System;
using System.Threading.Tasks;
using Back.Zone.Monads.EitherMonad;
using Back.Zone.Monads.OptionMonad;
using Back.Zone.Monads.TryMonad;

namespace Back.Zone.Monads.Validated;

public static class ValidatedExtensions
{
    public static Validated<Exception, TASuccessType> From<TASuccessType>(TASuccessType value) =>
        Try.From(value).ToValidated();

    public static async Task<Validated<Exception, TASuccessType>> FromAsync<TASuccessType>(
        Task<TASuccessType> valueTask) =>
        await Try.FromAsync(valueTask).ToValidatedAsync();

    public static Validated<TAFailureType, TBSuccessType> Map<TAFailureType, TASuccessType, TBSuccessType>(
        this Validated<TAFailureType, TASuccessType> validated,
        Func<TASuccessType, TBSuccessType> func
    ) => validated.Map(func);

    public static async Task<Validated<TAFailureType, TBSuccessType>> MapAsync<TAFailureType, TASuccessType,
        TBSuccessType>(
        this Task<Validated<TAFailureType, TASuccessType>> validatedTask,
        Func<TASuccessType, Task<TBSuccessType>> func
    )
    {
        var result = await validatedTask;

        return await result.MapAsync(func);
    }

    public static Validated<TAFailureType, TBSuccessType> Flatmap<TAFailureType, TASuccessType, TBSuccessType>(
        this Validated<TAFailureType, TASuccessType> validated,
        Func<TASuccessType, Validated<TAFailureType, TBSuccessType>> func
    ) => validated.Flatmap(func);

    public static async Task<Validated<TAFailureType, TBSuccessType>> FlatmapAsync<TAFailureType, TASuccessType,
        TBSuccessType>(
        this Task<Validated<TAFailureType, TASuccessType>> validatedTask,
        Func<TASuccessType, Task<Validated<TAFailureType, TBSuccessType>>> func
    )
    {
        var result = await validatedTask;

        return await result.FlatmapAsync(func);
    }

    public static TBSuccessType Fold<TAFailureType, TASuccessType, TBSuccessType>(
        this Validated<TAFailureType, TASuccessType> validated,
        Func<TAFailureType, TBSuccessType> failureFunc,
        Func<TASuccessType, TBSuccessType> successFunc
    ) => validated.Fold(failureFunc, successFunc);

    public static async Task<TBSuccessType> FoldAsync<TAFailureType, TASuccessType, TBSuccessType>(
        this Task<Validated<TAFailureType, TASuccessType>> validatedTask,
        Func<TAFailureType, Task<TBSuccessType>> failureFunc,
        Func<TASuccessType, TBSuccessType> successFunc
    ) => await (await validatedTask).FoldAsync(failureFunc, successFunc);

    public static async Task<TBSuccessType> FoldAsync<TAFailureType, TASuccessType, TBSuccessType>(
        this Task<Validated<TAFailureType, TASuccessType>> validatedTask,
        Func<TAFailureType, TBSuccessType> failureFunc,
        Func<TASuccessType, Task<TBSuccessType>> successFunc
    ) => await (await validatedTask).FoldAsync(failureFunc, successFunc);

    public static async Task<TBSuccessType> FoldAsync<TAFailureType, TASuccessType, TBSuccessType>(
        this Task<Validated<TAFailureType, TASuccessType>> validatedTask,
        Func<TAFailureType, Task<TBSuccessType>> failureFunc,
        Func<TASuccessType, Task<TBSuccessType>> successFunc
    ) => await (await validatedTask).FoldAsync(failureFunc, successFunc);

    public static Option<TASuccessType> ToOption<TAFailureType, TASuccessType>(
        this Validated<TAFailureType, TASuccessType> validated
    ) => validated.Fold<Option<TASuccessType>>(
        _ => new None<TASuccessType>(),
        success => new Some<TASuccessType>(success)
    );

    public static async Task<Option<TASuccessType>> ToOptionAsync<TAFailureType, TASuccessType>(
        this Task<Validated<TAFailureType, TASuccessType>> validatedTask
    ) => (await validatedTask).ToOption();

    public static Either<TAFailureType, TASuccessType> ToEither<TAFailureType, TASuccessType>(
        this Validated<TAFailureType, TASuccessType> validated
    ) => validated.Fold(
        failure => new Either<TAFailureType, TASuccessType>(failure),
        success => new Either<TAFailureType, TASuccessType>(success)
    );

    public static async Task<Either<TAFailureType, TASuccessType>> ToEitherAsync<TAFailureType, TASuccessType>(
        this Task<Validated<TAFailureType, TASuccessType>> validatedTask
    ) => (await validatedTask).ToEither();
}