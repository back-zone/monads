using System;
using System.Threading.Tasks;
using Back.Zone.Monads.OptionMonad;

namespace Back.Zone.Monads.EitherMonad;

public static class EitherExtentions
{
    public static TUnifiedType Fold<TUnifiedType>(
        this Either<TUnifiedType, TUnifiedType> either
    ) => either.Fold(l => l, r => r);

    public static async Task<Either<TLeftType, TRightTypeB>> MapAsync
        <TLeftType, TRightTypeA, TRightTypeB>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TRightTypeA, Task<TRightTypeB>> right
        )
    {
        var either = await eitherTask;
        return await either.MapAsync(right);
    }

    public static async Task<Either<TLeftType, TRightTypeB>> FlatmapAsync
        <TLeftType, TRightTypeA, TRightTypeB>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TRightTypeA, Task<Either<TLeftType, TRightTypeB>>> right
        )
    {
        var either = await eitherTask;
        return await either.FlatmapAsync(right);
    }

    public static async Task<TUnifiedType> FoldAsync
        <TLeftType, TRightTypeA, TUnifiedType>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TLeftType, Task<TUnifiedType>> left,
            Func<TRightTypeA, Task<TUnifiedType>> right
        )
    {
        var either = await eitherTask;
        return either.IsLeft ? await left(either.Left!) : await right(either.Right!);
    }

    public static async Task<TUnifiedType> FoldAsync
        <TLeftType, TRightTypeA, TUnifiedType>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TLeftType, TUnifiedType> left,
            Func<TRightTypeA, Task<TUnifiedType>> right
        )
    {
        var either = await eitherTask;
        return either.IsLeft ? left(either.Left!) : await right(either.Right!);
    }

    public static async Task<TUnifiedType> FoldAsync
        <TLeftType, TRightTypeA, TUnifiedType>
        (
            this Task<Either<TLeftType, TRightTypeA>> eitherTask,
            Func<TLeftType, Task<TUnifiedType>> left,
            Func<TRightTypeA, TUnifiedType> right
        )
    {
        var either = await eitherTask;
        return either.IsLeft ? await left(either.Left!) : right(either.Right!);
    }

    public static async Task<TUnifiedType> FoldAsync<TUnifiedType>(
        this Task<Either<TUnifiedType, TUnifiedType>> either
    )
    {
        var result = await either;
        return result.Fold(l => l, r => r);
    }

    public static Option<TRightType> ToOption<TLeftType, TRightType>(
        this Either<TLeftType, TRightType> either
    )
    {
        return either.Fold<Option<TRightType>>(_ => new None<TRightType>(), leftSide => new Some<TRightType>(leftSide));
    }

    public static async Task<Option<TRightType>> ToOptionAsync<TLeftType, TRightType>(
        this Task<Either<TLeftType, TRightType>> either
    )
    {
        var result = await either;
        return result.Fold<Option<TRightType>>(_ => new None<TRightType>(), leftSide => new Some<TRightType>(leftSide));
    }
}